using CommandLine;

namespace MOT.Publisher
{
    /// <summary>
    /// Configures FTP
    /// </summary>
    [Verb("ftp-config", HelpText = "Configures FTP")]
    class FTPConfigOptions
    {
        //Config doesnt need any options
    }

    /// <summary>
    /// Adds a file to FTP
    /// </summary>
    [Verb("ftp-addfile", HelpText = "Adds a file to FTP")]
    class FTPAddFileOptions
    {
        /// <summary>
        /// The local path to the file
        /// </summary>
        [Option('l', "localpath", Required = true, HelpText = "The local path to the file")]
        public string LocalPath { get; set; }

        /// <summary>
        /// The server path to save the file to
        /// </summary>
        [Option('s', "serverpath", Required = true, HelpText = "The server path to save the file to")]
        public string ServerPath { get; set; }
    }

    /// <summary>
    /// Deletes a file from FTP
    /// </summary>
    [Verb("ftp-deletefile", HelpText = "Deletes a file from FTP")]
    class FTPDeleteFileOptions
    {
        /// <summary>
        /// The server path to the file to delete
        /// </summary>
        [Option('s', "serverpath", Required = true, HelpText = "The server path to the file to delete")]
        public string ServerPath { get; set; }
    }

    /// <summary>
    /// Gets a file from FTP
    /// </summary>
    [Verb("ftp-getfile", HelpText = "Gets a file from FTP")]
    class FTPGetFileOptions
    {
        /// <summary>
        /// The server path to the file
        /// </summary>
        [Option('s', "serverpath", Required = true, HelpText = "The server path to the file")]
        public string ServerPath { get; set; }

        /// <summary>
        /// The local path to save the file to
        /// </summary>
        [Option('l', "localpath", Required = true, HelpText = "The local path to save the file to")]
        public string LocalPath { get; set; }
    }

    /// <summary>
    /// Adds a directory to FTP
    /// </summary>
    [Verb("ftp-adddirectory", HelpText = "Adds a directory to FTP")]
    class FTPAddDirectoryOptions
    {
        /// <summary>
        /// The server path to the directory to create
        /// </summary>
        [Option('s', "serverpath", Required = true, HelpText = "The server path to the directory to create")]
        public string ServerPath { get; set; }
    }

    /// <summary>
    /// Deletes a directory from FTP
    /// </summary>
    [Verb("ftp-deletedirectory", HelpText = "Deletes a directory from FTP")]
    class FTPDeleteDirectoryOptions
    {
        /// <summary>
        /// The server path to the directory to delete
        /// </summary>
        [Option('s', "serverpath", Required = true, HelpText = "The server path to the directory to delete")]
        public string ServerPath { get; set; }
    }
}
