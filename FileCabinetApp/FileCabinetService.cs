﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth)
        {
            var record = new FileCabinetRecord
            {
                Id = list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
            };

            this.list.Add(record);

            return record.Id;
        }
        public FileCabinetRecord[] GetRecords()
        {
            // TODO: Добавьте реализацию метода
            return Array.Empty<FileCabinetRecord>();
        }
        public int GetStat()
        {
            return this.list.Count;
        }
    }
}
