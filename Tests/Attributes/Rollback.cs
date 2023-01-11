using System;
using System.Transactions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TypingApp.Services.DatabaseProviders;

namespace Tests.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class Rollback : Attribute, ITestAction
{
    private TransactionScope? _scope;
    
    public ActionTargets Targets => ActionTargets.Test; 

    public void BeforeTest(ITest test)
    {
        _scope = new TransactionScope();
        BaseProvider.ResetDatabaseConnection();
    }

    public void AfterTest(ITest test)
    {
        // Neither of these work for some reason, even though they are called.
        _scope?.Dispose();
    }
}