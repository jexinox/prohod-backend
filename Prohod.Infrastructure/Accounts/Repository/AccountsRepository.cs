﻿using Kontur.Results;
using Microsoft.EntityFrameworkCore;
using Prohod.Domain.GenericRepository;
using Prohod.Domain.Users;
using Prohod.Infrastructure.Accounts.Models;
using Prohod.Infrastructure.Database;

namespace Prohod.Infrastructure.Accounts.Repository;

public class AccountsRepository : RepositoryBase<Account>, IAccountsRepository
{
    private readonly IAppDbContext dbContext;

    public AccountsRepository(IAppDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Result<EntityNotFoundError<Account>, User>> GetUserByLoginAndPassword(string login, string passwordsHash)
    {
        var user = await dbContext.Set<Account>()
            .Where(account => account.Login == login && account.PasswordHash == passwordsHash)
            .Select(account => account.AssociatedUser)
            .SingleOrDefaultAsync();

        return user is null ?
            new EntityNotFoundError<Account>("Account with provided login and password was not found") : user;
    }
}