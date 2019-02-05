﻿// https://docs.microsoft.com/en-us/visualstudio/modeling/t4-include-directive?view=vs-2017
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Solutions.Responses;

namespace ToDoSkill.Dialogs.DeleteToDo.Resources
{
    /// <summary>
    /// Contains bot responses.
    /// </summary>
    public class DeleteToDoResponses : IResponseIdCollection
    {
		public const string AfterTaskDeleted = "AfterTaskDeleted";
		public const string AfterAllTasksDeleted = "AfterAllTasksDeleted";
		public const string AskDeletionConfirmation = "AskDeletionConfirmation";
		public const string AskDeletionAllConfirmation = "AskDeletionAllConfirmation";    }
}