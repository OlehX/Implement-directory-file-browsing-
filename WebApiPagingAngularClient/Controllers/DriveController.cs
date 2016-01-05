using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using WebApiPagingAngularClient.Models;
using WebApiPagingAngularClient.Utility;

namespace WebApiPagingAngularClient.Controllers
{
    [RoutePrefix("api/drives")]
    public class DriveController : ApiController
    {
        private Drive drives;
        public DriveController() : this(new Drive())
        {

        }

        public DriveController(Drive dr)
        {
            this.drives = dr;
        }
      
        [Route("")]
        public IHttpActionResult Get()
        {
            var driveQuery = this.drives.Drives;
         

             driveQuery = driveQuery.OrderBy(c => c.Id);
           
            var drives = driveQuery.ToList();
            
            var result = new
            {
                Drives = drives
            };

            return Ok(result);
        }
       
       // GET: api/drives/name/pageSize/pageNumber/orderBy(optional)
        [Route("{name:alpha}/{pageSize:int}/{pageNumber:int}/{orderBy:alpha}")]
        public IHttpActionResult Get(string name, int pageSize, int pageNumber, string orderBy)
        {
            var path = new
            {
                Name = name
            
            };
            Dyrectory dyr = new Dyrectory(name + "://");
          
            var totalCount = dyr.Elements.Count();
            var totalPages = Math.Ceiling((double)totalCount / pageSize);

            var fileQuery = dyr.Elements;

            if (QueryHelper.PropertyExists<Elem>(orderBy))
            {
                var orderByExpression = QueryHelper.GetPropertyExpression<Elem>(orderBy);
                fileQuery = fileQuery.OrderBy(orderByExpression);
            }
            else
            {
                fileQuery = fileQuery.OrderBy(c => c.Type);
            }

            var elements = fileQuery.Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

            var result = new
            {
                Path = path,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Elements = elements
            };

            return Ok(result);
        }
        // GET: api/drives/name/pageSize/pageNumber/FolderPath/orderBy
        [Route("{name:alpha}/{pageSize:int}/{pageNumber:int}/{folderPath:minlength(0)}/{orderBy:alpha}")]
        public IHttpActionResult Get(string name, int pageSize, int pageNumber, string orderBy, string folderPath)
        {
            string parentFolder = "", urlPath = "", localPath = "";
       
                if (folderPath.Contains("-!"))
                {
                    urlPath = folderPath.Replace("-!", "/");
                    localPath = folderPath.Replace("-!", "\\");
                    parentFolder = folderPath.Substring(0, folderPath.LastIndexOf("-!"));
                }
                else
                {
                    urlPath = localPath = folderPath;
                }

                var path = new
                {
                    Name = name,
                    FolderPath = folderPath,
                    UrlPath = urlPath,
                    LocalPath = localPath,
                    ParentFolder = parentFolder
                };
                Dyrectory dyr = new Dyrectory(name + "://" + urlPath);

                var firstCount = dyr.CountFiles(-1, 10000000);
                var secondCount = dyr.CountFiles(10000000, 50000000);
                var thirdCount = dyr.CountFiles(100000000, -1);

                var countFiles = new
                {
                    FirstCount = firstCount,
                    SecondCount = secondCount,
                    ThirdCount = thirdCount
                };
                var totalCount = dyr.Elements.Count();
                var totalPages = Math.Ceiling((double)totalCount / pageSize);

                var fileQuery = dyr.Elements;

                if (QueryHelper.PropertyExists<Elem>(orderBy))
                {
                    var orderByExpression = QueryHelper.GetPropertyExpression<Elem>(orderBy);
                    fileQuery = fileQuery.OrderBy(orderByExpression);
                }
                else
                {
                    fileQuery = fileQuery.OrderBy(c => c.Type);
                }

                var elements = fileQuery.Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

                var result = new
                {
                    Path = path,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    Elements = elements,
                    countFiles = countFiles
                };
                return Ok(result);
            
      
        }

    }
}
