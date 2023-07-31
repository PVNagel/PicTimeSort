using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicTimeSortApplication.Model
{
    public class PictureRepository
    {
        private List<Picture> pictureList;

        public PictureRepository()
        {
            pictureList = new List<Picture>();
        }

        public void Add(Picture picture)
        {
            pictureList.Add(picture);
        }

        public void AddRange(List<Picture> pictureBatch)
        {
            pictureList.AddRange(pictureBatch);
        }

        public List<Picture> GetAll() 
        { 
            return pictureList; 
        }
    }
}
