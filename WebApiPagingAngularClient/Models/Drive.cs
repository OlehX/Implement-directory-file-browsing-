using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApiPagingAngularClient.Models
{
    public class Drive
    {

        private List<DriveModels> drives = new List<DriveModels>();

       
        public IEnumerable<DriveModels> GetAll { get { return this.drives; } }

        public Drive()
        {
            DriveInfo[] di = DriveInfo.GetDrives();
            int i = 0;
            foreach (DriveInfo drives in di)
            {
                    if (drives.IsReady && drives.DriveFormat!="HFS"){                

                    this.drives.Add(new DriveModels { Id = i, Name = drives.Name.First().ToString(), Type = drives.DriveType.ToString() });
                    i++;
                    }
            
            }

            
        }
        public IQueryable<DriveModels> Drives
        {
            get { return this.GetAll.AsQueryable(); }
        }
        public List<DriveModels> GetDriveByName(string name)
        {
            if (name.Length > 3)
            {
                return GetDriveByType(name);
            }
            else
            {
                string n = name.ToUpper();
                return drives.Where(o => o.Name == n).ToList();
            }
        }
        public List<DriveModels> GetDriveByType(string type)
        {
            return drives.Where(o => o.Type.ToLower().Equals(type.ToLower())).ToList();
        }
        public DriveModels GetDriveById(int id)
        {
            return drives.Find(o => o.Id == id);
        }
       
    }
}