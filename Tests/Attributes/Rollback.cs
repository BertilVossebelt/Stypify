using System;
using System.Transactions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Tests.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class Rollback : Attribute, ITestAction
{
    private TransactionScope _scope;
    
    public void BeforeTest(ITest test)
    {
        _scope = new TransactionScope();  
    }

    public void AfterTest(ITest test)
    {
        Transaction.Current?.Rollback();
        _scope.Dispose();  
    }

    public ActionTargets Targets => ActionTargets.Test; 
}