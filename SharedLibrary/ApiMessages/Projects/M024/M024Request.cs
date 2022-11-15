using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.M024;

public record M024Request(string Value) : IRequest<IResult<Guid>>;
