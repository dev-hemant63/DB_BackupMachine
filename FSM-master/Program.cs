using FSM.Repository;
using System;

class Program
{
    static void Main(string[] args)
    {
        DatabaseBackupStateMachine fsm = new DatabaseBackupStateMachine();

        fsm.RequestBackup();

        Console.ReadLine();
    }
}
