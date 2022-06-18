using RabbitCrypt.Bitmap;
using RabbitCrypt.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitCrypt
{
    public static class ImageEngine //legacy
    {
        private async static Task<Image<Rgb24>?> EncodeSync(Image<Rgb24> original, string inputText, string password)
        {
            if (original.Width % 9 != 0 | original.Height % 16 != 0) return null;

            decimal[] keys = Generer.GetKeysImg(password);
            string[] text = ArrayOperations.Secut(inputText, 8);

            int[] length = new int[8];
            for (int i = 0; i < 8; i++) length[i] = text[i].Length;

            var clusters = new long[8][];
            int seed = Generer.GetSeed(keys);


            await Dispatcher.Multithread(8, t => clusters[t] = Generer.CreateClusters(length[t], keys[t]));

            var ShuffleClusters = new List<long>();
            foreach (long[] clusters0 in clusters)
            {
                foreach (long cluster in clusters0)
                    ShuffleClusters.Add(cluster);
            }

            var Semiconductor = new Random(seed);
            long[] SClusters = ShuffleClusters.OrderBy(x => Semiconductor.Next()).ToArray<Int64>();
            long[][] ClustersArray = ArrayOperations.Secut(SClusters, 8);
            string[] pixelCode = new string[8]; string output = "";
            Random[] rnd = new Random[8]; Random kurwa = new Random();
            for (int i = 0; i < 8; i++) rnd[i] = new Random(kurwa.Next(0, Int32.MaxValue));

            await Dispatcher.Multithread(8, t => pixelCode[t] = PixelCode.Emission(ClustersArray[t], text[t], rnd[t]));
            for (int i = 0; i < 7; i++) output += pixelCode[i] + ";";
            output += pixelCode[7];
            string[] Emission = output.Split(';');
            var result = new Image<Rgb24>(original.Width, original.Height);
            int pcc = 0; var pc = new List<string>();
            foreach (string item in Emission)
            {
                string[] q = item.Split(',');
                string[] w = q[7].Split('|');
                for (int i = 0; i < 7; i++) pc.Add(q[i]);
                foreach (string w0 in w) pc.Add(w0);
            }
            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    Rgb24 orig = original[x, y];
                    if (pcc < pc.Count)
                    { result[x, y] = Comparer.Emission(orig, pc[pcc]); pcc++; }
                    else { result[x, y] = Comparer.Noise(orig, kurwa); }
                }
            }
            return result;
        }
        private static async Task<string?> DecodeSync(Image<Rgb24> original, Image<Rgb24> comparable, string password)
        {
            if (original.Width % 9 != 0 | original.Height % 16 != 0) return null;
            if (comparable.Width % 9 != 0 | comparable.Height % 16 != 0) return null;
            string[] dpc = new string[16]; string dpcResult = "";
            var O = new Image<Rgb24>[16]; var C = new Image<Rgb24>[16];

            for (byte i = 0; i < 16; i++)
            {
                O[i] = original.Clone();
                C[i] = comparable.Clone();
            }

            await Dispatcher.Multithread(16, t => dpc[t] = Detecter.GetPixelCode(O[t], C[t], t));

            foreach (string pc in dpc) dpcResult += pc;
            string[] pixelCode = dpcResult.Substring(0, dpcResult.Length - 1).Split(';');
            if (original.Width % 9 != 0 | original.Height % 16 != 0) return null;
            decimal[] keys = Generer.GetKeysImg(password);

            string[][] text = ArrayOperations.Secut(pixelCode, 8);
            int[] length = new int[8];
            for (int i = 0; i < 8; i++) length[i] = text[i].Length;
            var clusters = new long[8][];
            int seed = Generer.GetSeed(keys);
            await Dispatcher.Multithread(8, t => clusters[t] = Generer.CreateClusters(length[t], keys[t]));
            var ShuffleClusters = new List<long>();
            foreach (long[] clusters0 in clusters)
            {
                foreach (long cluster in clusters0)
                    ShuffleClusters.Add(cluster);
            }
            Random Semiconductor = new Random(seed);
            long[] SClusters = ShuffleClusters.OrderBy(x => Semiconductor.Next()).ToArray<Int64>();
            long[][] ClustersArray = ArrayOperations.Secut(SClusters, 8);
            string[] result = new string[8]; var output = new StringBuilder();
            await Dispatcher.Multithread(8, t => result[t] = PixelCode.Detect(ClustersArray[t], text[t]));
            foreach (string thread in result) output.Append(thread);
            return output.ToString();
        }

        /// <summary>
        /// Осуществляет асинхронное шифрование информации на изображение
        /// </summary>
        /// <param name="original">Растровый ключ</param>
        /// <param name="inputText">Текс для шифрования</param>
        /// <param name="password">Текстовый ключ</param>
        /// <returns>Изображение, промодулированое шифром</returns>
        public async static Task<Image<Rgb24>?> EncodeAsync(Image<Rgb24> original, string inputText, string password) =>
            await Task.Run(() => EncodeSync(original, inputText, password));

        /// <summary>
        /// Осущесвтляет асинхронное дешифрование информации с изображения
        /// </summary>
        /// <param name="original">Растровый ключ</param>
        /// <param name="comparable">Изображение, промодулированое шифром</param>
        /// <param name="password">Текстовый ключ</param>
        /// <returns>Дешифрованный текст</returns>
        public static async Task<string?> DecodeAsync(Image<Rgb24> original, Image<Rgb24> comparable, string password) =>
            await Task.Run(() => DecodeSync(original, comparable, password));
    }
}
