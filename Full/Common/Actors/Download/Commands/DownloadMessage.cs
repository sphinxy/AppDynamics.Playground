namespace Common.Actors.Download.Commands
{
    public sealed class DownloadMessage
    {
        public DownloadMessage(string baseAddress, string relativePath)
        {
            BaseAddress = baseAddress;
            RelativePath = relativePath;
        }

        public string BaseAddress { get; }
        public string RelativePath { get; }
    }
}
