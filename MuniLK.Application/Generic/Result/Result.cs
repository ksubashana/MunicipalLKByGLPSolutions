using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuniLK.Application.Generic.Result
{
    public class Result
    {
        public bool Succeeded { get; init; }
        public string? Error { get; init; }

        public static Result Success() => new() { Succeeded = true };
        public static Result Failure(string error) => new() { Succeeded = false, Error = error };
    }

    public class Result<T> : Result
    {
        public T? Data { get; init; }
        public T? ExistingData { get; init; }

        // Factory methods
        public static Result<T> Success(T data) => new() { Succeeded = true, Data = data, ExistingData = default };
        public static Result<T> Failure(string error) => new() { Succeeded = false, Error = error, ExistingData = default };
        public static Result<T> Failure(string error, T existingData) => new() { Succeeded = false, Error = error, ExistingData = existingData };
    }

    public record PagedResult<T>(IReadOnlyList<T> Items, int Total, int Page, int PageSize);
}
