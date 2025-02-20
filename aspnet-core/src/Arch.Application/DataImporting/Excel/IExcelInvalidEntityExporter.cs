﻿using System.Collections.Generic;
using Abp.Dependency;
using Arch.Dto;

namespace Arch.DataImporting.Excel;

public interface IExcelInvalidEntityExporter<TEntityDto> : ITransientDependency
{
    FileDto ExportToFile(List<TEntityDto> entities);
}