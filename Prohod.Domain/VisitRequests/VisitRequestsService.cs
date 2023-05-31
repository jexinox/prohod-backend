﻿using Kontur.Results;
using Prohod.Domain.ErrorsBase;
using Prohod.Domain.Forms;
using Prohod.Domain.GenericRepository;
using Prohod.Domain.Users;

namespace Prohod.Domain.VisitRequests;

public class VisitRequestsService : IVisitRequestsService
{
    private readonly IVisitRequestsRepository visitRequestsRepository;
    private readonly IFormsRepository formsRepository;
    private readonly IUsersRepository usersRepository;

    public VisitRequestsService(
        IVisitRequestsRepository visitRequestsRepository,
        IFormsRepository formsRepository,
        IUsersRepository usersRepository)
    {
        this.visitRequestsRepository = visitRequestsRepository;
        this.formsRepository = formsRepository;
        this.usersRepository = usersRepository;
    }

    public async Task<Result<EntityNotFoundError<User>>> ApplyFormAsync(ApplyFormDto applyFormDto)
    {
        var (passport, visitTime, visitReason, userToVisitId, emailToSendReply) = applyFormDto;
        var findUserResult = await usersRepository.FindAsync(userToVisitId);
        if (findUserResult.TryGetFault(out var fault, out var user))
        {
            return fault;
        }

        var form = new Form(passport, visitTime, visitReason, user, emailToSendReply);
        await formsRepository.AddAsync(form);
        await visitRequestsRepository.AddAsync(new VisitRequest(form));
        return Result.Succeed();
    }

    public async Task<Result<IOperationError>> AcceptRequestAsync(Guid visitRequestId, Guid whoAcceptedId)
    {
        var findVisitRequestResult = await visitRequestsRepository.FindAsync(visitRequestId);
        if (findVisitRequestResult.TryGetFault(out var visitRequestNotFound, out var visitRequest))
        {
            return visitRequestNotFound;
        }

        var findUserResult = await usersRepository.FindAsync(whoAcceptedId);
        if (findUserResult.TryGetFault(out var userNotFound, out var user))
        {
            return userNotFound;
        }
        
        return visitRequest.AcceptRequest(user).TryGetFault(out var acceptVisitRequestError) 
            ? acceptVisitRequestError 
            : Result.Succeed();
    }
    
    public async Task<Result<IOperationError>> RejectRequestAsync(
        Guid visitRequestId, Guid whoProcessedId, string rejectionReason)
    {
        var findVisitRequestResult = await visitRequestsRepository.FindAsync(visitRequestId);
        if (findVisitRequestResult.TryGetFault(out var visitRequestNotFound, out var visitRequest))
        {
            return visitRequestNotFound;
        }
        
        var findUserResult = await usersRepository.FindAsync(whoProcessedId);
        if (findUserResult.TryGetFault(out var userNotFound, out var user))
        {
            return userNotFound;
        }

        return visitRequest.RejectRequest(user, rejectionReason).TryGetFault(out var rejectVisitRequestError)
            ? rejectVisitRequestError
            : Result.Succeed();
    }

    public async Task<IReadOnlyList<VisitRequest>> GetNotProcessedVisitRequestsPage(int offset, int limit)
    {
        return await visitRequestsRepository.GetNotProcessedVisitRequestsPageAsync(offset, limit);
    }

    public async Task<IReadOnlyList<VisitRequest>> GetVisitRequestsPage(int offset, int limit)
    {
        return await visitRequestsRepository.GetVisitRequestsPageAsync(offset, limit);
    }

    public async Task<Result<EntityNotFoundError<User>, IReadOnlyList<VisitRequest>>> GetUserProcessedVisitRequestsPage(
        Guid userId, int offset, int limit)
    {
        var userExists = await usersRepository.ExistsAsync(userId);

        return userExists
            ? Result.Succeed(
                await visitRequestsRepository.GetUserProcessedVisitRequestsPageAsync(userId, offset, limit))
            : EntityNotFoundError<User>.FromId(userId);
    }
}