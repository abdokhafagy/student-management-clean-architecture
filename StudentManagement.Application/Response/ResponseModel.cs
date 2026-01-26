using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Application.Response;

public class ResponseModel
{
    public bool IsSuccess { get; set; }
    public string? message { get; set; }
}
public class ResponseDataModel<T> : ResponseModel
{
    public T? data { get; set; }
}


public class ResponseIdModel : ResponseModel
{
    public string? Id { get; set; }
}
