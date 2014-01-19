using System;
using System.Drawing;
using System.IO;

namespace Application.Infrastructure.FilesManagement
{
	public class AttachedFile
	{
		#region Constructors

		public AttachedFile()
		{
		}

		public AttachedFile(string dipslayName, string guid, FileInfo fileInfo, int id, string deleteType)
		{
			SetValues(dipslayName, guid, fileInfo, (int)fileInfo.Length, fileInfo.FullName, id, deleteType);
		}

		public AttachedFile(string displayName, string guid, FileInfo fileName, int fileLength, string fullPath, string deleteType)
		{
			SetValues(displayName, guid, fileName, fileLength, fullPath, -1, deleteType);
		}

		#endregion Constructors

		#region FilesStatus Members

		public string DeleteType
		{
			get;
			set;
		}

		public string DeleteUrl
		{
			get;
			set;
		}

		public string Error
		{
			get;
			set;
		}

		public string Group
		{
			get;
			set;
		}

		public int IdFile
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Progress
		{
			get;
			set;
		}

		public int Size
		{
			get;
			set;
		}

		public string ThumbnailUrl
		{
			get;
			set;
		}

		public string Type
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public string GuidFileName
		{
			get;
			set;
		}

		#endregion FilesStatus Members

		#region Fields

		public const string HandlerPath = "/";

		#endregion Fields

		#region Private Members

		private static double ConvertBytesToMegabytes(long bytes)
		{
			return (bytes / 1024f) / 1024f;
		}

		private string EncodeImage(string fileName)
		{
			byte[] bytes;
			using (Image image = Image.FromFile(fileName))
			{
				var ratioX = (double)80 / image.Width;
				var ratioY = (double)80 / image.Height;
				var ratio = Math.Min(ratioX, ratioY);
				var newWidth = (int)(image.Width * ratio);
				var newHeight = (int)(image.Height * ratio);
				var newImage = new Bitmap(newWidth, newHeight);
				Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
				var converter = new ImageConverter();
				bytes = (byte[])converter.ConvertTo(newImage, typeof(byte[]));
				newImage.Dispose();
			}

			return Convert.ToBase64String(bytes);
		}

		private bool _IsAudio(string ext)
		{
			return ext == ".mp3" || ext == ".wav";
		}

		private bool _IsImage(string ext)
		{
			return ext == ".gif" || ext == ".jpg" || ext == ".png" || ext == ".bmp";
		}

		private bool _IsVideo(string ext)
		{
			return ext == ".mp4" || ext == ".m4v" || ext == ".ogg" || ext == ".ogv" || ext == ".flv" || ext == ".mov";
		}

		private void SetValues(string displayName, string guid, FileInfo fileName, int fileLength, string fullPath, int id, string deleteType)
		{
			IdFile = id;
			Name = displayName;
			Size = fileLength;
			DeleteType = deleteType;
			Progress = "1.0";
			GuidFileName = guid;

			var extension = Path.GetExtension(fullPath);
			if (extension != null)
			{
				var ext = extension.ToLower();
				var fileSize = ConvertBytesToMegabytes(new FileInfo(fullPath).Length);
				if (fileSize > 10 || !_IsImage(ext))
				{
					if (_IsAudio(ext))
					{
						Type = "Audio";
						ThumbnailUrl = "/Content/mvcfileupload/img/generalAudio.png";
					}
					else if (_IsVideo(ext))
					{
						Type = "Video";
						ThumbnailUrl = "/Content/mvcfileupload/img/generalVideo.png";
					}
					else
					{
						Type = "Document";
						ThumbnailUrl = "/Content/mvcfileupload/img/generalFile.png";
					}
				}
				else
				{
					Type = "Image";
					ThumbnailUrl = @"data:image/png;base64," + EncodeImage(fullPath);
				}
			}

			var serverFilePath = Type + "/" + Path.GetFileName(fileName.FullName);
			Url = HandlerPath + "api/Upload?filename=/" + serverFilePath;
			DeleteUrl = HandlerPath + "api/Upload?filename=" + serverFilePath;
		}

		#endregion Private Members
	}
}