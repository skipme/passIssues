using System;
using System.Collections.Generic;
using System.Linq;

/*
 * > В таблице размером 3x3, проставлены в произвольном порядке цифры от 1 до 9.
> Требуется последовательно обойти все элементы этой таблицы таким образом,
> чтобы получить на выходе максимальное число, сформированное из цифр
> пройденных ячеек.
> Обход можно начинать с произвольной ячейки, перемещаться можно на соседнюю
> ячейку только по горизонтали и вертикали, запрещено заходить в одну и ту же
> ячейку более одного раза.
>  
> Входные данные, три строки, с цифрами разделенными пробелами, например
> 1 2 3
> 4 5 6
> 9 8 7
>  
> Программа, должна вывести результат в виде числа, например
> 987654123
*/
namespace testCrossInform
{
    class Program
    {
        static void Main(string[] args)
        {
            table tx = null;
            
            Console.Write("enter rows or press enter\n");
            string[] input = new string[3];
            input[0] = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input[0]))
                tx = new table();
            else
            {
                input[2] = Console.ReadLine();
                input[1] = Console.ReadLine();
                tx = new table(input);
                Console.WriteLine();
            }
            Console.WriteLine("table: ");
            tx.PrintStdOut();
            TraverseTable(tx);
            Console.WriteLine();
            Console.WriteLine("result:");
            Console.WriteLine(tx.DumpHistoryNum());
            // метод формирования максимального числа из пройденых не уточнен, вот два
            Console.WriteLine(tx.DumpHistoryNumSorted());
            Console.WriteLine(tx.DumpHistoryNumSortedString());
        }
        static void TraverseTable(table t)
        {
            while (true)
            {
                int[] steps = t.GetAvailableSteps().ToArray();
                if (steps.Length == 0)
                    break;

                int sel = steps[0];
                t.MakeStep(sel);
            }
        }
    }
    class table
    {
        readonly static int[] primitive = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int[] instanced;
        public void PrintStdOut()
        {
            Console.WriteLine($"{instanced[0]} {instanced[1]} {instanced[2]}");
            Console.WriteLine($"{instanced[3]} {instanced[4]} {instanced[5]}");
            Console.WriteLine($"{instanced[6]} {instanced[7]} {instanced[8]}");
        }
        public table()
        {
            Random rnd = new Random();
            instanced = new int[primitive.Length];
            Array.Copy(primitive, instanced, primitive.Length);
            for (int i = 0; i < instanced.Length; i++)
            {
                int index = rnd.Next(i, primitive.Length);
                int temp = instanced[i];
                instanced[i] = instanced[index];
                instanced[index] = temp;
            }

            StepsHistory = new SortedSet<int>();
            StepHistoryNumbers = new List<int>();
            this.MakeStep(rnd.Next(0, primitive.Length));
        }
        public table(string[] rows)
        {
            if (rows.Length != 3)
                throw new Exception();
            instanced = new int[9];
            for (int i = 0; i < 3; i++)
            {
                string[] coln = rows[i].Split(' ');
                if (coln.Length != 3) throw new Exception($"неверное количество чисел/пробелов в строке №{i + 1}");
                try
                {
                    instanced[i * 3] = int.Parse(coln[0]);
                    instanced[i * 3 + 1] = int.Parse(coln[1]);
                    instanced[i * 3 + 2] = int.Parse(coln[2]);
                }
                catch (Exception __)
                {
                    throw new Exception($"неверное число в строке №{i + 1}: {__.Message}");
                }
            }
            Random rnd = new Random();
            StepsHistory = new SortedSet<int>();
            StepHistoryNumbers = new List<int>();
            this.MakeStep(rnd.Next(0, primitive.Length));
        }

        int currentStep;
        SortedSet<int> StepsHistory;
        List<int> StepHistoryNumbers;
        /*
         * a b c
         * x y z
         * i j k
         */
private List<int> AvailableSteps()
        {
            int line = (int)Math.Floor(currentStep / 3.0f);
            int column = currentStep % 3;
            bool right = column < 2,
                left = column > 0,
                up = line > 0,
                down = line < 2;
            List<int> result = new List<int>();
            if (right) result.Add(currentStep + 1);
            if (left) result.Add(currentStep - 1);
            if (up) result.Add(currentStep - 3);
            if (down) result.Add(currentStep + 3);

            return result;
        }
        public IEnumerable<int> GetAvailableSteps()
        {
            List<int> tableAvailabled = AvailableSteps();
            for (int i = 0; i < tableAvailabled.Count; i++)
            {
                if (!StepsHistory.Contains(tableAvailabled[i]))
                    yield return tableAvailabled[i];
            }
        }
        public void MakeStep(int TableIndex)
        {
            if (TableIndex < 0 && TableIndex >= 9)
                throw new Exception();

            // с этими начальными ячейками может не пройти всю таблицу
            if (StepsHistory.Count == 0 && (TableIndex == 3 || TableIndex == 5 || TableIndex == 1 || TableIndex == 7))
                TableIndex--;

            StepsHistory.Add(TableIndex);
            currentStep = TableIndex;
            StepHistoryNumbers.Add(instanced[TableIndex]);
        }
        public string DumpHistoryNum()
        {
            return String.Join("", this.StepHistoryNumbers);
        }
        public string DumpHistoryNumSorted()
        {
            return String.Join("", from n in this.StepHistoryNumbers orderby n descending select n);
        }

        internal string DumpHistoryNumSortedString()
        {
            return String.Join("", from n in this.StepHistoryNumbers orderby n.ToString() descending select n);
        }
    }
}
