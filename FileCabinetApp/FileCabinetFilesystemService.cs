﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    internal class FileCabinetFilesystemService : IFileCabinetService
    {
        private FileStream fileStream;

        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        // TODO : Refator method
        public int CreateRecord(FileCabinetRecord record)
        {
            fileStream.Position = fileStream.Length;

            fileStream.Write(new byte[2], 0, 2);

            byte[] idBytes = new byte[4];
            int id = GetStat() + 1;
            Encoding.Default.GetBytes(id.ToString()).CopyTo(idBytes, 0);
            fileStream.Write(idBytes, 0, idBytes.Length);

            byte[] firstName = new byte[120];
            Encoding.Default.GetBytes(record.FirstName).CopyTo(firstName, 0);
            fileStream.Write(firstName, 0, firstName.Length);

            byte[] lastName = new byte[120];
            Encoding.Default.GetBytes(record.LastName).CopyTo(lastName, 0);
            fileStream.Write(lastName, 0, lastName.Length);

            byte[] year = new byte[4];
            Encoding.Default.GetBytes(record.DateOfBirth.Year.ToString()).CopyTo(year, 0);
            fileStream.Write(year, 0, year.Length);

            byte[] month = new byte[4];
            Encoding.Default.GetBytes(record.DateOfBirth.Month.ToString()).CopyTo(month, 0);
            fileStream.Write(month, 0, month.Length);

            byte[] day = new byte[4];
            Encoding.Default.GetBytes(record.DateOfBirth.Day.ToString()).CopyTo(day, 0);
            fileStream.Write(day, 0, day.Length);

            byte[] age = new byte[2];
            Encoding.Default.GetBytes(record.Age.ToString()).CopyTo(age, 0);
            fileStream.Write(age, 0, age.Length);

            byte[] money = new byte[16];
            Encoding.Default.GetBytes(record.Money.ToString()).CopyTo(money, 0);
            fileStream.Write(money, 0, money.Length);

            byte[] letter = new byte[1];
            Encoding.Default.GetBytes(record.Letter.ToString()).CopyTo(letter, 0);
            fileStream.Write(letter, 0, letter.Length);

            return id;
        }

        // TODO : Refator method
        public void EditRecord(int id, FileCabinetRecord newRecord)
        {
            int i = 0;

            while (i < fileStream.Length)
            {
                fileStream.Position = i + 2;
                byte[] idBytes = new byte[4];
                fileStream.Read(idBytes, 0, idBytes.Length);

                if (id == Convert.ToInt32(Encoding.Default.GetString(idBytes)))
                {
                    fileStream.Position = i + 6;
                    byte[] firstNameBytes = new byte[120];
                    Encoding.Default.GetBytes(newRecord.FirstName).CopyTo(firstNameBytes, 0);
                    fileStream.Write(firstNameBytes, 0, firstNameBytes.Length);

                    fileStream.Position = i + 126;
                    byte[] lastNameBytes = new byte[120];
                    Encoding.Default.GetBytes(newRecord.LastName).CopyTo(lastNameBytes, 0);
                    fileStream.Write(lastNameBytes, 0, lastNameBytes.Length);

                    fileStream.Position = i + 246;
                    byte[] yearBytes = new byte[4];
                    Encoding.Default.GetBytes(newRecord.DateOfBirth.Year.ToString()).CopyTo(yearBytes, 0);
                    fileStream.Write(yearBytes, 0, yearBytes.Length);

                    fileStream.Position = i + 250;
                    byte[] monthBytes = new byte[4];
                    Encoding.Default.GetBytes(newRecord.DateOfBirth.Month.ToString()).CopyTo(monthBytes, 0);
                    fileStream.Write(monthBytes, 0, monthBytes.Length);

                    fileStream.Position = i + 254;
                    byte[] dayBytes = new byte[4];
                    Encoding.Default.GetBytes(newRecord.DateOfBirth.Day.ToString()).CopyTo(dayBytes, 0);
                    fileStream.Write(dayBytes, 0, dayBytes.Length);

                    fileStream.Position = i + 258;
                    byte[] ageBytes = new byte[2];
                    Encoding.Default.GetBytes(newRecord.Age.ToString()).CopyTo(ageBytes, 0);
                    fileStream.Write(ageBytes, 0, ageBytes.Length);

                    fileStream.Position = i + 260;
                    byte[] moneyBytes = new byte[16];
                    Encoding.Default.GetBytes(newRecord.Money.ToString()).CopyTo(moneyBytes, 0);
                    fileStream.Write(moneyBytes, 0, moneyBytes.Length);

                    fileStream.Position = i + 276;
                    byte[] letterBytes = new byte[1];
                    Encoding.Default.GetBytes(newRecord.Letter.ToString()).CopyTo(letterBytes, 0);
                    fileStream.Write(letterBytes, 0, letterBytes.Length);
                }

                i += 277;
            }
        }

        // TODO : Refator method
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var list = new List<FileCabinetRecord>();

            int i = 0;

            while (i < fileStream.Length)
            {
                fileStream.Position = i + 6;
                byte[] firstNameBytes = new byte[120];
                fileStream.Read(firstNameBytes, 0, firstNameBytes.Length);

                if (firstName == Encoding.Default.GetString(firstNameBytes).Replace("\0", string.Empty))
                {
                    fileStream.Position = i + 2;
                    byte[] idBytes = new byte[4];
                    fileStream.Read(idBytes, 0, idBytes.Length);
                    int id = Convert.ToInt32(Encoding.Default.GetString(idBytes));

                    fileStream.Position = i + 126;
                    byte[] lastNameBytes = new byte[120];
                    fileStream.Read(lastNameBytes, 0, lastNameBytes.Length);
                    string lastName = Encoding.Default.GetString(lastNameBytes).Replace("\0", string.Empty);

                    fileStream.Position = i + 246;
                    byte[] yearBytes = new byte[4];
                    fileStream.Read(yearBytes, 0, yearBytes.Length);
                    int year = Convert.ToInt32(Encoding.Default.GetString(yearBytes));

                    fileStream.Position = i + 250;
                    byte[] monthBytes = new byte[4];
                    fileStream.Read(monthBytes, 0, monthBytes.Length);
                    int month = Convert.ToInt32(Encoding.Default.GetString(monthBytes));

                    fileStream.Position = i + 254;
                    byte[] dayBytes = new byte[4];
                    fileStream.Read(dayBytes, 0, dayBytes.Length);
                    int day = Convert.ToInt32(Encoding.Default.GetString(dayBytes));

                    fileStream.Position = i + 258;
                    byte[] ageBytes = new byte[2];
                    fileStream.Read(ageBytes, 0, ageBytes.Length);
                    short age = Convert.ToInt16(Encoding.Default.GetString(ageBytes));

                    fileStream.Position = i + 260;
                    byte[] moneyBytes = new byte[16];
                    fileStream.Read(moneyBytes, 0, moneyBytes.Length);
                    decimal money = Convert.ToDecimal(Encoding.Default.GetString(moneyBytes));

                    fileStream.Position = i + 276;
                    byte[] letterBytes = new byte[1];
                    fileStream.Read(letterBytes, 0, letterBytes.Length);
                    char letter = Convert.ToChar(Encoding.Default.GetString(letterBytes));

                    var record = new FileCabinetRecord()
                    {
                        Id = id,
                        FirstName = firstName,
                        LastName = lastName,
                        DateOfBirth = new DateTime(year, month, day),
                        Age = age,
                        Money = money,
                        Letter = letter,
                    };

                    list.Add(record);
                }

                i += 277;
            }

            return list.AsReadOnly();
        }

        // TODO : Refactor method
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            var list = new List<FileCabinetRecord>();

            int i = 0;

            while (i < fileStream.Length)
            {
                fileStream.Position = i + 126;
                byte[] lastNameBytes = new byte[120];
                fileStream.Read(lastNameBytes, 0, lastNameBytes.Length);

                if (lastName == Encoding.Default.GetString(lastNameBytes).Replace("\0", string.Empty))
                {
                    fileStream.Position = i + 2;
                    byte[] idBytes = new byte[4];
                    fileStream.Read(idBytes, 0, idBytes.Length);
                    int id = Convert.ToInt32(Encoding.Default.GetString(idBytes));

                    fileStream.Position = i + 6;
                    byte[] firstNameBytes = new byte[120];
                    fileStream.Read(firstNameBytes, 0, firstNameBytes.Length);
                    string firstName = Encoding.Default.GetString(firstNameBytes).Replace("\0", string.Empty);

                    fileStream.Position = i + 246;
                    byte[] yearBytes = new byte[4];
                    fileStream.Read(yearBytes, 0, yearBytes.Length);
                    int year = Convert.ToInt32(Encoding.Default.GetString(yearBytes));

                    fileStream.Position = i + 250;
                    byte[] monthBytes = new byte[4];
                    fileStream.Read(monthBytes, 0, monthBytes.Length);
                    int month = Convert.ToInt32(Encoding.Default.GetString(monthBytes));

                    fileStream.Position = i + 254;
                    byte[] dayBytes = new byte[4];
                    fileStream.Read(dayBytes, 0, dayBytes.Length);
                    int day = Convert.ToInt32(Encoding.Default.GetString(dayBytes));

                    fileStream.Position = i + 258;
                    byte[] ageBytes = new byte[2];
                    fileStream.Read(ageBytes, 0, ageBytes.Length);
                    short age = Convert.ToInt16(Encoding.Default.GetString(ageBytes));

                    fileStream.Position = i + 260;
                    byte[] moneyBytes = new byte[16];
                    fileStream.Read(moneyBytes, 0, moneyBytes.Length);
                    decimal money = Convert.ToDecimal(Encoding.Default.GetString(moneyBytes));

                    fileStream.Position = i + 276;
                    byte[] letterBytes = new byte[1];
                    fileStream.Read(letterBytes, 0, letterBytes.Length);
                    char letter = Convert.ToChar(Encoding.Default.GetString(letterBytes));

                    var record = new FileCabinetRecord()
                    {
                        Id = id,
                        FirstName = firstName,
                        LastName = lastName,
                        DateOfBirth = new DateTime(year, month, day),
                        Age = age,
                        Money = money,
                        Letter = letter,
                    };

                    list.Add(record);
                }

                i += 277;
            }

            return list.AsReadOnly();
        }

        // TODO : Refator method
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string inputDateOfBirth)
        {
            var list = new List<FileCabinetRecord>();

            int i = 0;

            while (i < fileStream.Length)
            {
                fileStream.Position = i + 246;
                byte[] yearBytes = new byte[4];
                fileStream.Read(yearBytes, 0, yearBytes.Length);
                int year = Convert.ToInt32(Encoding.Default.GetString(yearBytes));

                fileStream.Position = i + 250;
                byte[] monthBytes = new byte[4];
                fileStream.Read(monthBytes, 0, monthBytes.Length);
                int month = Convert.ToInt32(Encoding.Default.GetString(monthBytes));

                fileStream.Position = i + 254;
                byte[] dayBytes = new byte[4];
                fileStream.Read(dayBytes, 0, dayBytes.Length);
                int day = Convert.ToInt32(Encoding.Default.GetString(dayBytes));

                if (Convert.ToDateTime(inputDateOfBirth) == new DateTime(year, month, day))
                {
                    fileStream.Position = i + 2;
                    byte[] idBytes = new byte[4];
                    fileStream.Read(idBytes, 0, idBytes.Length);
                    int id = Convert.ToInt32(Encoding.Default.GetString(idBytes));

                    fileStream.Position = i + 6;
                    byte[] firstNameBytes = new byte[120];
                    fileStream.Read(firstNameBytes, 0, firstNameBytes.Length);
                    string firstName = Encoding.Default.GetString(firstNameBytes).Replace("\0", string.Empty);

                    fileStream.Position = i + 126;
                    byte[] lastNameBytes = new byte[120];
                    fileStream.Read(lastNameBytes, 0, lastNameBytes.Length);
                    string lastName = Encoding.Default.GetString(lastNameBytes).Replace("\0", string.Empty);

                    fileStream.Position = i + 258;
                    byte[] ageBytes = new byte[2];
                    fileStream.Read(ageBytes, 0, ageBytes.Length);
                    short age = Convert.ToInt16(Encoding.Default.GetString(ageBytes));

                    fileStream.Position = i + 260;
                    byte[] moneyBytes = new byte[16];
                    fileStream.Read(moneyBytes, 0, moneyBytes.Length);
                    decimal money = Convert.ToDecimal(Encoding.Default.GetString(moneyBytes));

                    fileStream.Position = i + 276;
                    byte[] letterBytes = new byte[1];
                    fileStream.Read(letterBytes, 0, letterBytes.Length);
                    char letter = Convert.ToChar(Encoding.Default.GetString(letterBytes));

                    var record = new FileCabinetRecord()
                    {
                        Id = id,
                        FirstName = firstName,
                        LastName = lastName,
                        DateOfBirth = new DateTime(year, month, day),
                        Age = age,
                        Money = money,
                        Letter = letter,
                    };

                    list.Add(record);
                }

                i += 277;
            }

            return list.AsReadOnly();
        }

        // TODO : Refator method
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var list = new List<FileCabinetRecord>();

            int i = 0;

            while (i < fileStream.Length)
            {
                fileStream.Position = i + 2;
                byte[] idBytes = new byte[4];
                fileStream.Read(idBytes, 0, idBytes.Length);
                int id = Convert.ToInt32(Encoding.Default.GetString(idBytes));

                fileStream.Position = i + 6;
                byte[] firstNameBytes = new byte[120];
                fileStream.Read(firstNameBytes, 0, firstNameBytes.Length);
                string firstName = Encoding.Default.GetString(firstNameBytes).Replace("\0", string.Empty);

                fileStream.Position = i + 126;
                byte[] lastNameBytes = new byte[120];
                fileStream.Read(lastNameBytes, 0, lastNameBytes.Length);
                string lastName = Encoding.Default.GetString(lastNameBytes).Replace("\0", string.Empty);

                fileStream.Position = i + 246;
                byte[] yearBytes = new byte[4];
                fileStream.Read(yearBytes, 0, yearBytes.Length);
                int year = Convert.ToInt32(Encoding.Default.GetString(yearBytes));

                fileStream.Position = i + 250;
                byte[] monthBytes = new byte[4];
                fileStream.Read(monthBytes, 0, monthBytes.Length);
                int month = Convert.ToInt32(Encoding.Default.GetString(monthBytes));

                fileStream.Position = i + 254;
                byte[] dayBytes = new byte[4];
                fileStream.Read(dayBytes, 0, dayBytes.Length);
                int day = Convert.ToInt32(Encoding.Default.GetString(dayBytes));

                fileStream.Position = i + 258;
                byte[] ageBytes = new byte[2];
                fileStream.Read(ageBytes, 0, ageBytes.Length);
                short age = Convert.ToInt16(Encoding.Default.GetString(ageBytes));

                fileStream.Position = i + 260;
                byte[] moneyBytes = new byte[16];
                fileStream.Read(moneyBytes, 0, moneyBytes.Length);
                decimal money = Convert.ToDecimal(Encoding.Default.GetString(moneyBytes));

                fileStream.Position = i + 276;
                byte[] letterBytes = new byte[1];
                fileStream.Read(letterBytes, 0, letterBytes.Length);
                char letter = Convert.ToChar(Encoding.Default.GetString(letterBytes));

                var record = new FileCabinetRecord()
                {
                    Id = id,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = new DateTime(year, month, day),
                    Age = age,
                    Money = money,
                    Letter = letter,
                };

                list.Add(record);

                i += 277;
            }

            return list.AsReadOnly();
        }

        // TODO : Refactor method
        public int GetStat()
        {
            return (int)fileStream.Length / 277;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
