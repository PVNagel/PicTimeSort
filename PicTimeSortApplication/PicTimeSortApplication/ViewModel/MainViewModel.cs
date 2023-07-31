using PicTimeSortApplication.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicTimeSortApplication.ViewModel
{
    public class MainViewModel
    {
        PictureRepository pictureRepo = new PictureRepository();
        public string SelectedFolder { get; set; }

        public void SelectFolder()
        {
            using (var FileExplorerDialog = new FolderBrowserDialog())
            {
                FileExplorerDialog.ShowDialog();
                SelectedFolder = FileExplorerDialog.SelectedPath;
            }
        }

        public void ReadFolder()
        {
            string[] imageFiles = Directory.GetFiles(SelectedFolder, "*.*", SearchOption.TopDirectoryOnly).Where(file =>
            file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
            file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
            file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
            file.EndsWith(".heic", StringComparison.OrdinalIgnoreCase) ||
            file.EndsWith(".dng", StringComparison.OrdinalIgnoreCase) ||
            file.EndsWith(".arw", StringComparison.OrdinalIgnoreCase) ||
            file.EndsWith(".sr2", StringComparison.OrdinalIgnoreCase) ||
            file.EndsWith(".cr2", StringComparison.OrdinalIgnoreCase)).ToArray();

            int batchSize = 10;

            for (int i = 0; i < imageFiles.Length; i += batchSize)
            {
                List<Picture> batchPictures = new List<Picture>();
                int upperBound = Math.Min(i + batchSize, imageFiles.Length);

                for (int j = i; j < upperBound; j++)
                {
                    string imagePath = imageFiles[j];
                    Picture picture = new Picture();
                    using (Image image = Image.FromFile(imagePath))
                    {
                        picture.PictureName = Path.GetFileName(imagePath);
                        picture.DateTaken = GetImageDateTaken(image);
                    }
                    batchPictures.Add(picture);
                }

                pictureRepo.AddRange(batchPictures);
            }
        }

        private DateTime GetImageDateTaken(Image image)
        {
            try
            {
                PropertyItem propertyItem = image.GetPropertyItem(0x9003); // 0x9003 is the identifier for DateTaken property
                string dateTaken = Encoding.UTF8.GetString(propertyItem.Value).TrimEnd('\0');
                return DateTime.ParseExact(dateTaken, "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
            catch
            {
                return default;
            }
        }

        public void SortFolder()
        {
            List<Picture> pictures = pictureRepo.GetAll();
            pictures.Sort((p1, p2) => p1.DateTaken.CompareTo(p2.DateTaken));

            string sortedFolderPath = Path.Combine(SelectedFolder, "SortedPictures");
            Directory.CreateDirectory(sortedFolderPath);

            // Move the sorted pictures to the 'sorted' directory
            foreach (Picture picture in pictures)
            {
                string subfolderName = picture.DateTaken.ToString("yyyy-MM-dd");
                string subfolderPath = Path.Combine(sortedFolderPath, subfolderName);
                Directory.CreateDirectory(subfolderPath);

                string sourceFilePath = Path.Combine(SelectedFolder, picture.PictureName);
                string destinationFilePath = Path.Combine(subfolderPath, picture.PictureName);
                File.Move(sourceFilePath, destinationFilePath);
            }
        }
    }
}
