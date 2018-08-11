using Microsoft.VisualBasic;
using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using CommandLine;
using CommandLine.Text;
using Limilabs.FTP.Client;

namespace MOT.Publisher
{
    /// <summary>
    /// The command line parser
    /// </summary>
    class CommandLineParser
    {
        /// <summary>
        /// The argument parser
        /// </summary>
        Parser _argumentParser;

        /// <summary>
        /// The result of the parse
        /// </summary>
        ParserResult<object> _parseResult;

        /// <summary>
        /// Parses the command line arguments
        /// </summary>
        /// <param name="args">The command line aruguments</param>
        public int Parse(string[] args)
        {
            _argumentParser = new Parser();
            _parseResult = _argumentParser.ParseArguments<
                FTPConfigOptions,
                FTPAddFileOptions,
                FTPDeleteFileOptions,
                FTPGetFileOptions,
                FTPAddDirectoryOptions,
                FTPDeleteDirectoryOptions
                >(args);
            return _parseResult.MapResult(
                (FTPConfigOptions opts) => FTPConfig(opts),
                (FTPAddFileOptions opts) => FTPAddFile(opts),
                (FTPDeleteFileOptions opts) => FTPDeleteFile(opts),
                (FTPGetFileOptions opts) => FTPGetFile(opts),
                (FTPAddDirectoryOptions opts) => FTPAddDirectory(opts),
                (FTPDeleteDirectoryOptions opts) => FTPDeleteDirectory(opts),
                errs => Errors(errs)
                );
        }

        /// <summary>
        /// Configures FTP
        /// </summary>
        /// <param name="opts">The options</param>
        /// <returns>The result</returns>
        int FTPConfig(FTPConfigOptions opts)
        {
            string server = Properties.Settings.Default.FTPServer;
            server = Interaction.InputBox("Please enter the FTP server:", "Mist of Time Publisher", server);
            Properties.Settings.Default.FTPServer = server;

            string username = Properties.Settings.Default.FTPUsername;
            username = Interaction.InputBox("Please enter the FTP username:", "Mist of Time Publisher", username);
            Properties.Settings.Default.FTPUsername = username;

            System.Security.SecureString password = CommandLineSecurity.DecryptString(Properties.Settings.Default.FTPPassword);
            password = CommandLineSecurity.ToSecureString(Interaction.InputBox("Please enter the FTP password:", "Mist of Time Publisher", CommandLineSecurity.ToInsecureString(password)));
            Properties.Settings.Default.FTPPassword = CommandLineSecurity.EncryptString(password);

            Properties.Settings.Default.Save();

            return 0;
        }

        /// <summary>
        /// Adds a file to FTP
        /// </summary>
        /// <param name="opts">The options</param>
        /// <returns>The result</returns>
        int FTPAddFile(FTPAddFileOptions opts)
        {
            using (Ftp ftp = new Ftp())
            {
                try
                {
                    ftp.Connect(Properties.Settings.Default.FTPServer);
                    ftp.Login(Properties.Settings.Default.FTPUsername, CommandLineSecurity.ToInsecureString(CommandLineSecurity.DecryptString(Properties.Settings.Default.FTPPassword)));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(new Form(), ex.Message, "Mist of Time Publisher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                }

                try
                {
                    if (!ftp.FolderExists("cdn"))
                    {
                        ftp.CreateFolder("cdn");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(new Form(), ex.Message, "Mist of Time Publisher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                }

                try
                {
                    if (!opts.ServerPath.StartsWith("/"))
                    {
                        opts.ServerPath = "/" + opts.ServerPath;
                    }
                    ftp.Upload("cdn" + opts.ServerPath, opts.LocalPath);
                    string oldPath = Path.GetDirectoryName("cdn" + opts.ServerPath).Replace('\\', '/') + "/FtpTrial-" + Path.GetFileName("cdn" + opts.ServerPath);
                    if (ftp.FileExists(oldPath))
                    {
                        ftp.Rename(oldPath, "cdn" + opts.ServerPath);
                    }
                }
                catch (Exception)
                {
                    return 2;
                }
            }

            return 0;
        }

        /// <summary>
        /// Deletes a file from FTP
        /// </summary>
        /// <param name="opts">The options</param>
        /// <returns>The result</returns>
        int FTPDeleteFile(FTPDeleteFileOptions opts)
        {
            using (Ftp ftp = new Ftp())
            {
                try
                {
                    ftp.Connect(Properties.Settings.Default.FTPServer);
                    ftp.Login(Properties.Settings.Default.FTPUsername, CommandLineSecurity.ToInsecureString(CommandLineSecurity.DecryptString(Properties.Settings.Default.FTPPassword)));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(new Form(), ex.Message, "Mist of Time Publisher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                }

                try
                {
                    if (!ftp.FolderExists("cdn"))
                    {
                        ftp.CreateFolder("cdn");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(new Form(), ex.Message, "Mist of Time Publisher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                }

                try
                {
                    if (!opts.ServerPath.StartsWith("/"))
                    {
                        opts.ServerPath = "/" + opts.ServerPath;
                    }
                    ftp.DeleteFile("cdn" + opts.ServerPath);
                }
                catch (Exception)
                {
                    return 2;
                }
            }

            return 0;
        }

        /// <summary>
        /// Gets a file from FTP
        /// </summary>
        /// <param name="opts">The options</param>
        /// <returns>The result</returns>
        int FTPGetFile(FTPGetFileOptions opts)
        {
            using (Ftp ftp = new Ftp())
            {
                try
                {
                    ftp.Connect(Properties.Settings.Default.FTPServer);
                    ftp.Login(Properties.Settings.Default.FTPUsername, CommandLineSecurity.ToInsecureString(CommandLineSecurity.DecryptString(Properties.Settings.Default.FTPPassword)));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(new Form(), ex.Message, "Mist of Time Publisher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                }

                try
                {
                    if (!ftp.FolderExists("cdn"))
                    {
                        ftp.CreateFolder("cdn");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(new Form(), ex.Message, "Mist of Time Publisher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                }

                try
                {
                    if (!opts.ServerPath.StartsWith("/"))
                    {
                        opts.ServerPath = "/" + opts.ServerPath;
                    }
                    File.WriteAllBytes(opts.LocalPath, ftp.Download("cdn" + opts.ServerPath));
                }
                catch (Exception)
                {
                    return 2;
                }
            }

            return 0;
        }

        /// <summary>
        /// Adds a directory to FTP
        /// </summary>
        /// <param name="opts">The options</param>
        /// <returns>The result</returns>
        int FTPAddDirectory(FTPAddDirectoryOptions opts)
        {
            using (Ftp ftp = new Ftp())
            {
                try
                {
                    ftp.Connect(Properties.Settings.Default.FTPServer);
                    ftp.Login(Properties.Settings.Default.FTPUsername, CommandLineSecurity.ToInsecureString(CommandLineSecurity.DecryptString(Properties.Settings.Default.FTPPassword)));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(new Form(), ex.Message, "Mist of Time Publisher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                }

                try
                {
                    if (!ftp.FolderExists("cdn"))
                    {
                        ftp.CreateFolder("cdn");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(new Form(), ex.Message, "Mist of Time Publisher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                }

                try
                {
                    if (!opts.ServerPath.StartsWith("/"))
                    {
                        opts.ServerPath = "/" + opts.ServerPath;
                    }
                    ftp.CreateFolder("cdn" + opts.ServerPath);
                }
                catch (Exception)
                {
                    return 2;
                }
            }

            return 0;
        }

        /// <summary>
        /// Deletes a directory from FTP
        /// </summary>
        /// <param name="opts">The options</param>
        /// <returns>The result</returns>
        int FTPDeleteDirectory(FTPDeleteDirectoryOptions opts)
        {
            using (Ftp ftp = new Ftp())
            {
                try
                {
                    ftp.Connect(Properties.Settings.Default.FTPServer);
                    ftp.Login(Properties.Settings.Default.FTPUsername, CommandLineSecurity.ToInsecureString(CommandLineSecurity.DecryptString(Properties.Settings.Default.FTPPassword)));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(new Form(), ex.Message, "Mist of Time Publisher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                }

                try
                {
                    if (!ftp.FolderExists("cdn"))
                    {
                        ftp.CreateFolder("cdn");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(new Form(), ex.Message, "Mist of Time Publisher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                }

                try
                {
                    if (!opts.ServerPath.StartsWith("/"))
                    {
                        opts.ServerPath = "/" + opts.ServerPath;
                    }
                    ftp.DeleteFolder("cdn" + opts.ServerPath);
                }
                catch (Exception)
                {
                    return 2;
                }
            }

            return 0;
        }

        /// <summary>
        /// Parses errors
        /// </summary>
        /// <param name="errs">The errors</param>
        /// <returns>The result</returns>
        int Errors(IEnumerable<Error> errs)
        {
            foreach (Error item in errs)
            {
                if (item.Tag == ErrorType.VersionRequestedError)
                {
                    MessageBox.Show(new Form(), HelpText.AutoBuild(_parseResult), "Mist of Time Publisher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 0;
                }
                else if (item.Tag == ErrorType.HelpRequestedError || item.Tag == ErrorType.HelpVerbRequestedError)
                {
                    MessageBox.Show(new Form(), HelpText.AutoBuild(_parseResult), "Mist of Time Publisher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 0;
                }
                else
                {
                    MessageBox.Show(new Form(), HelpText.AutoBuild(_parseResult), "Mist of Time Publisher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                }
            }

            return 1;
        }
    }
}
