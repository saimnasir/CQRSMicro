﻿using Patika.Framework.Domain.Interfaces.Repository;

namespace CQRSMicro.Sale.DBContext.Interfaces
{
    public interface ISaleCUDRepository : IGenericRepository<Entities.Sale, Guid>
    {
    }
}
