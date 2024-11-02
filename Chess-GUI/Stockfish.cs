using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;

namespace Chess_GUI
{
    
    public class Stockfish
    {
        public static bool IsStockfishInitialized { get; private set; } = false;
        string stockfishPath = @"D:\Faks\Diplomski CHESS-GUI\Chess-GUI\stockfish\stockfish-windows-x86-64-avx2.exe";
        private Process stockfish;
        private StreamWriter inputWriter;
        private StreamReader outputReader;
        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        public bool Start()
        {
            try
            {
                stockfish = new Process();
                stockfish.StartInfo.FileName = stockfishPath;
                stockfish.StartInfo.UseShellExecute = false;
                stockfish.StartInfo.RedirectStandardInput = true;
                stockfish.StartInfo.RedirectStandardOutput = true;
                stockfish.StartInfo.CreateNoWindow = true;

                stockfish.Start();
                inputWriter = stockfish.StandardInput;
                outputReader = stockfish.StandardOutput;
               
                if (stockfish == null)
                    throw new InvalidOperationException("Stockfish engine is not running.");

                inputWriter.WriteLine("uci");
                string output = "";
                        
                while (!output.Contains("uciok"))
                {
                    output += outputReader.ReadLine() + "\n";
                }
                inputWriter.WriteLine("setoption name Threads value 4");
                return output.Contains("uciok");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting Stockfish: {ex.Message}");
                return false;
            }
        }

        public async Task<string> BestMoves(string fen, TextBlock textblock, ScrollViewer scroll)
        {
            if (stockfish == null)
                throw new InvalidOperationException("Stockfish engine is not running.");
            List<string> topMoves = new List<string>();
            await inputWriter.WriteLineAsync($"position fen {fen}");
            await inputWriter.WriteLineAsync("go depth 15");

            while (true)
            {
                string line = await outputReader.ReadLineAsync().ConfigureAwait(false);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    textblock.Text += line + "\n";
                    scroll.ScrollToEnd();
                });
                if (line.Contains("bestmove"))
                {
                    return line.Substring(9, 4);
                }
            }
            return "";
        }
        public void Stop()
        {
            if (stockfish != null)
            {
                stockfish.Close();
            }
        }
    }
}
