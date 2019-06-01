using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneBook
{
    public class PhoneBookAccess
    {
        PhoneBook.pbEntities ent = new pbEntities();
        public PB_RECORDS TakeRecord(int id)
        {
            return TakeRecord(id, ent);
        }

        public PB_RECORDS TakeRecord(int id, pbEntities ent)
        {
            PB_RECORDS rec = ent.PB_RECORDS.Where(o => o.PBR_ID == id).FirstOrDefault();
            if (rec == null)
            {
                throw new Exception(string.Format("record with id:{0} does not persist", id));
            }
            return rec;
        }
        public void ModifyRecord(int id, string name, string familyName, string phoneNumber)
        {
            PB_RECORDS rec = TakeRecord(id);
            rec.PBR_NAME = name;
            rec.PBR_PHONE_NUMBER = phoneNumber;
            rec.PBR_FAMILY_NAME = familyName;
            ent.SaveChanges();
        }
        public int AddRecord(string name, string familyName, string phoneNumber)
        {
            PB_RECORDS rec = ent.PB_RECORDS.Add(new PB_RECORDS
            {
                PBR_FAMILY_NAME = familyName,
                PBR_NAME = name,
                PBR_PHONE_NUMBER = phoneNumber
            });
            ent.SaveChanges();
            return rec.PBR_ID;
        }
        public IEnumerable<PB_RECORDS> GetDataFilter(string containsString)
        {
            var result = from x in ent.PB_RECORDS
                         where x.PBR_FAMILY_NAME.Contains(containsString) || x.PBR_NAME.Contains(containsString)
                         select x;
            return result;
        }
        public IEnumerable<PB_RECORDS> GetData()
        {
            var result = from x in ent.PB_RECORDS
                         select x;
            return result;
        }
        public byte[] GetXlsData()
        {
            //
            foreach (var row in GetData())
            {
                // добавляем строку в xls файл
            }
            //
            return null;// возвращает xls файл
        }
        public byte[] GetCsvData()
        {
            StringBuilder sb = new StringBuilder();

            //
            foreach (var row in GetData())
            {
                // добавляем строку в csv файл
                sb.AppendFormat("{0},{1},{2}\r\n", row.PBR_FAMILY_NAME, row.PBR_NAME, row.PBR_PHONE_NUMBER);
            }
            //
            return Encoding.Unicode.GetBytes(sb.ToString());// возвращает csv файл
        }
    }
}
