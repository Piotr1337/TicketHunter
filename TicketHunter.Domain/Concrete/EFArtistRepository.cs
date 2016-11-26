using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
//using AutoMapper;
using TicketHunter.Domain.Abstract;
using TicketHunter.Domain.Entities;

namespace TicketHunter.Domain.Concrete
{
    public class EFArtistRepository : IArtistRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Artists> Artists
        {
            get
            {
                return context.Artists;
            }
        }

        public IEnumerable<SelectListItem> ArtistsForDropList
        {
            get
            {
                var selectListItems = new List<SelectListItem>();
                var selectListItems2 = new List<SelectListItem>();

                var artists = new SelectListGroup() {Name = "Artysta"};
                var bands = new SelectListGroup() { Name = "Zespół" };

                selectListItems = Artists.Where(x => x.IsBand).Select(artist => new SelectListItem() {Text = artist.Nickname, Value = artist.ArtistID.ToString(),  Group = bands}).ToList();
                selectListItems2 = Artists.Where(x => x.IsBand == false).Select(band => new SelectListItem() {Text = band.Nickname, Value = band.ArtistID.ToString(), Group = artists}).ToList();

                return selectListItems.Concat(selectListItems2);
            }
        }

        public void SaveArtist(Artists theArtist)
        {
            try
            {
                if (theArtist.ArtistID == 0)
                {
                    context.Artists.Add(theArtist);
                }
                else
                {
                    Artists dbEntry = context.Artists.Find(theArtist.ArtistID);
                    if (dbEntry != null)
                    {
                        var mapping = Mapper.Map(theArtist, dbEntry);
                        if (!string.IsNullOrEmpty(theArtist.ImageMimeType))
                        {
                            mapping.ImageData = theArtist.ImageData;
                            mapping.ImageMimeType = theArtist.ImageMimeType;
                        }
                    }
                }
                context.SaveChanges();
            }
            catch(DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }
           
        }

        public Artists GetArtists(int artistId)
        {
            var foundArtist = context.Artists.Find(artistId);
            return foundArtist;
        }

        public Artists DeleteArtist(int artistId)
        {
            Artists dbEntry = context.Artists.Find(artistId);
            if (dbEntry != null)
            {
                context.Artists.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

       
    }
}
