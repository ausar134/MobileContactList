using System;
using System.Collections.Generic;
using System.Text;

namespace ContactBook.Models
{
    public static class LogMessages
    {
        public const string DeletingContact = "Trying to delete contact {@Contact}";
        public const string TriedToDeleteNonExistingContact = "User tried to delete non existing contact, with parameter on API call {Id}";
    }
}
