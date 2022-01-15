using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weelo.Microservices.Properties.Domain.DTOS
{
    public class PaginationMetadataDTO
    {
        public PaginationMetadataDTO(int totalCount, int currentPage, int itemsPerPage) { 
            TotalCount = totalCount;
            Currrentpage = currentPage;
            TotalPages = (int)Math.Ceiling(totalCount / (double)itemsPerPage);
        }

        public int Currrentpage { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages  { get; set; }

        public bool HasPrevious => Currrentpage > 1;
        public bool HasNext => Currrentpage < TotalPages;

    }
}
