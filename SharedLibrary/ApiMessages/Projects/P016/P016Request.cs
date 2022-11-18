using MediatR;
using SharedLibrary.Wrapper;

namespace SharedLibrary.ApiMessages.Projects.P016;

public record P016Request(string Value) : IRequest<IResult<Guid>>;
