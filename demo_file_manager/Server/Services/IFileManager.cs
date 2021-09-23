using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace demo_file_manager.Server.Services
{
    public interface IFileManager
    {
        Task<byte[]> GetDataAsBytes(string filePath);
        FileStream GetDataAsStream(string filePath);
        Task<byte[]> ReadAsync(string filePath);
    }

    public class FileManager : IFileManager
    {
        public async Task<byte[]> GetDataAsBytes(string filePath) => await File.ReadAllBytesAsync(filePath);
        public FileStream GetDataAsStream(string filePath) => File.OpenRead(filePath);

        public async Task<byte[]> ReadAsync(string filePath)
        {
            byte[] result;

            using (FileStream SourceStream = File.Open(filePath, new FileStreamOptions() { Mode = FileMode.Open, Options = FileOptions.Asynchronous }))
            {
                result = new byte[SourceStream.Length];
                await SourceStream.ReadAsync(result, 0, (int)SourceStream.Length);
            }

            return result;
        }
    }
}
