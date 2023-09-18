using OfficeOpenXml;
using OfficeOpenXml.Table;
using OSIET_Finance.Models.Finance;
using System.Collections;

namespace OSIET_Finance
{
    public class Utils
    {
        public static ExcelPackage DownloadJournalInTwoDate(List<Journals> fModels, DateTime date1, DateTime date2)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var path = Path.Combine(Environment.CurrentDirectory, "wwwroot", "Template", "mensuel.xlsx");
            var package = new ExcelPackage(path);

            //Add a new worksheet to the empty workbook
            var worksheet = package.Workbook.Worksheets.First();

            //Defining the tables parameters
            int firstRow = 3;
            int lastRow = 3 + fModels.Count;
            int firstColumn = 1;
            int lastColumn = 17;
            ExcelRange rg = worksheet.Cells[firstRow, firstColumn, lastRow, lastColumn];
            string tableName = "Tableau1";
            //Ading a table to a Range
            //ExcelTable tab = worksheet.Tables.Add(rg, tableName);
            ExcelTable tab = worksheet.Tables.Where(x => x.Name == tableName).First();
            //Formating the table style
            //tab.TableStyle = TableStyles.Light8;

            //DATE DU RAPPORT
            //worksheet.Cells[1, 1].Value = "Date debut du rapport :";
            if (fModels.Count() > 0)
            {
                worksheet.Cells[2, 3].Value = date1.ToString("dd MMMM yyyy");
            }
            //worksheet.Cells[2, 1].Value = "Date fin du rapport : ";
            if (fModels.Count() > 0)
            {
                worksheet.Cells[3, 3].Value = date2.ToString("dd MMMM yyyy");
            }
            int i = 6;
            foreach (var fModel in fModels)
            {
                tab.AddRow(i + 1);
                worksheet.Cells[i + 1, 1].Value = fModel.Date.ToString("dd MMMM yyyy");
                worksheet.Cells[i + 1, 2].Value = fModel.PayerPar;
                worksheet.Cells[i + 1, 3].Value = fModel.ImputationNom;
                worksheet.Cells[i + 1, 4].Value = fModel.RefPiece;
                worksheet.Cells[i + 1, 5].Value = fModel.Libelle;
                worksheet.Cells[i + 1, 6].Value = fModel.Concernee;
                if (fModel.CompteNom == "CAISSE" || fModel.CompteNom == "caisse" || fModel.CompteNom == "Caisse")
                {
                    if (fModel.Type == "Entrée")
                        worksheet.Cells[i + 1, 7].Value = fModel.Montant;
                    else if (fModel.Type == "Sortie")
                        worksheet.Cells[i + 1, 8].Value = fModel.Montant;
                }
                else if (fModel.CompteNom == "B.N.I" || fModel.CompteNom == "bni" || fModel.CompteNom == "BNI")
                {
                    if (fModel.Type == "Entrée")
                        worksheet.Cells[i + 1, 9].Value = fModel.Montant;
                    else if (fModel.Type == "Sortie")
                        worksheet.Cells[i + 1, 10].Value = fModel.Montant;
                }
                else if (fModel.CompteNom == "B.F.V" || fModel.CompteNom == "bfv" || fModel.CompteNom == "BFV")
                {
                    if (fModel.Type == "Entrée")
                        worksheet.Cells[i + 1, 11].Value = fModel.Montant;
                    else if (fModel.Type == "Sortie")
                        worksheet.Cells[i + 1, 12].Value = fModel.Montant;
                }
                else if (fModel.CompteNom == "B.O.A" || fModel.CompteNom == "boa" || fModel.CompteNom == "BOA")
                {
                    if (fModel.Type == "Entrée")
                        worksheet.Cells[i + 1, 13].Value = fModel.Montant;
                    else if (fModel.Type == "Sortie")
                        worksheet.Cells[i + 1, 14].Value = fModel.Montant;
                }
                i++;
            }

            return package;
        }

        public static ExcelPackage DownloadJournalAnnuel(List<Journals> fModels, int annee, double lastYear)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var path = Path.Combine(Environment.CurrentDirectory, "wwwroot", "Template", "Annuel.xlsx");
            var package = new ExcelPackage(path);

            //Add a new worksheet to the empty workbook
            var worksheet = package.Workbook.Worksheets.First();

            //Defining the tables parameters
            int firstRow = 3;
            int lastRow = 3 + fModels.Count;
            int firstColumn = 1;
            int lastColumn = 17;
            ExcelRange rg = worksheet.Cells[firstRow, firstColumn, lastRow, lastColumn];
            string tableRecette = "TabRecettes";
            string tableDepense = "TabDepenses";
            string tableToRecette = "TotauxRecette";
            string tableToDepnse = "TotauxDepense";

            ExcelTable tabRec = worksheet.Tables.Where(x => x.Name == tableRecette).First();
            ExcelTable tabDep = worksheet.Tables.Where(x => x.Name == tableDepense).First();
            ExcelTable tabTRec = worksheet.Tables.Where(x => x.Name == tableToRecette).First();
            ExcelTable tabTDep = worksheet.Tables.Where(x => x.Name == tableToDepnse).First();
            //DATE DU RAPPORT
            worksheet.Cells[3, 3].Value = lastYear;

            int r = 4;
            int d = 0;
            int i = 0;
            List<Journals> listNew = new List<Journals>();
            Dictionary<string, List<ListMonthInYear>> listImputSortie = new Dictionary<string, List<ListMonthInYear>>();
            Dictionary<string, List<ListMonthInYear>> listImputEntree = new Dictionary<string, List<ListMonthInYear>>();
            ///ENTREE
            foreach (var fModel in fModels)
            {
                if (fModel.ImputationNom == "Virement interne")
                    continue;
                try
                {
                    Journals j = new Journals();
                    j = listNew.Where(x => x.ImputationId == fModel.ImputationId && x.Date.Month == fModel.Date.Month && x.Type == "Entrée").First();
                    listNew.Remove(j);
                    j.Montant += fModel.Montant;
                    listNew.Add(j);
                    List<ListMonthInYear> lst = new List<ListMonthInYear>();
                    ListMonthInYear lstImYear = new();
                    lstImYear.type = j.Type;
                    lstImYear.month = j.Date.Month;
                    lstImYear.montant = j.Montant;
                    lst.Add(lstImYear);
                    if (listImputEntree.ContainsKey(j.ImputationNom))
                    {
                        List<ListMonthInYear> m = listImputEntree[j.ImputationNom];
                        ListMonthInYear p = new();
                        try
                        {
                            p = m.Where(x => x.month == lstImYear.month).First();
                            m.Remove(p);
                            m.Add(lstImYear);
                            listImputEntree[j.ImputationNom] = m;
                        }
                        catch (InvalidOperationException)
                        {
                            m.Add(lstImYear);
                            listImputEntree[j.ImputationNom] = m;
                        }
                    }
                    else
                        listImputEntree.Add(j.ImputationNom, lst);

                }
                catch (InvalidOperationException)
                {
                    if (fModel.Type == "Entrée")
                    {
                        listNew.Add(fModel);
                        List<ListMonthInYear> lst = new List<ListMonthInYear>();
                        ListMonthInYear lstImYear = new();
                        lstImYear.type = fModel.Type;
                        lstImYear.month = fModel.Date.Month;
                        lstImYear.montant = fModel.Montant;
                        lst.Add(lstImYear);
                        if (listImputEntree.ContainsKey(fModel.ImputationNom))
                        {
                            List<ListMonthInYear> m = listImputEntree[fModel.ImputationNom];
                            m.Add(lstImYear);
                            listImputEntree[fModel.ImputationNom] = m;
                        }
                        else
                            listImputEntree.Add(fModel.ImputationNom, lst);
                    }

                }
            }
            ///SORTIE
            foreach (var fModel in fModels)
            {
                if (fModel.ImputationNom == "Virement interne")
                    continue;
                try
                {
                    Journals j = new Journals();
                    j = listNew.Where(x => x.ImputationId == fModel.ImputationId && x.Date.Month == fModel.Date.Month && x.Type == "Sortie").First();
                    listNew.Remove(j);
                    j.Montant += fModel.Montant;
                    listNew.Add(j);
                    List<ListMonthInYear> lst = new List<ListMonthInYear>();
                    ListMonthInYear lstImYear = new();
                    lstImYear.type = j.Type;
                    lstImYear.month = j.Date.Month;
                    lstImYear.montant = j.Montant;
                    lst.Add(lstImYear);
                    if (listImputSortie.ContainsKey(j.ImputationNom))
                    {
                        List<ListMonthInYear> m = listImputSortie[j.ImputationNom];
                        ListMonthInYear p = new();
                        try
                        {
                            p = m.Where(x => x.month == lstImYear.month).First();
                            m.Remove(p);
                            m.Add(lstImYear);
                            listImputSortie[j.ImputationNom] = m;
                        }
                        catch (InvalidOperationException)
                        {
                            m.Add(lstImYear);
                            listImputSortie[j.ImputationNom] = m;
                        }
                    }
                    else
                        listImputSortie.Add(j.ImputationNom, lst);

                }
                catch (InvalidOperationException)
                {
                    if (fModel.Type == "Sortie")
                    {
                        listNew.Add(fModel);
                        List<ListMonthInYear> lst = new List<ListMonthInYear>();
                        ListMonthInYear lstImYear = new();
                        lstImYear.type = fModel.Type;
                        lstImYear.month = fModel.Date.Month;
                        lstImYear.montant = fModel.Montant;
                        lst.Add(lstImYear);
                        if (listImputSortie.ContainsKey(fModel.ImputationNom))
                        {
                            List<ListMonthInYear> m = listImputSortie[fModel.ImputationNom];
                            m.Add(lstImYear);
                            listImputSortie[fModel.ImputationNom] = m;
                        }
                        else
                            listImputSortie.Add(fModel.ImputationNom, lst);
                    }

                }
            }
            int e = 4;

            //ADDING RECETTES TO EXCEL FILE
            foreach (string key in listImputEntree.Keys)
            {
                List<ListMonthInYear> list = listImputEntree[key];
                if (e == 4)
                {
                    worksheet.Cells[e + 1, 1].Value = key;
                    int range = e + 1;
                    worksheet.Cells[e + 1, 2].Formula = "=SUM(C" + range + ":N" + range + ")";
                    foreach (var item in list)
                    {
                        if (item.month == 1)
                            worksheet.Cells[e + 1, 3].Value = item.montant;
                        if (item.month == 2)
                            worksheet.Cells[e + 1, 4].Value = item.montant;
                        if (item.month == 3)
                            worksheet.Cells[e + 1, 5].Value = item.montant;
                        if (item.month == 4)
                            worksheet.Cells[e + 1, 6].Value = item.montant;
                        if (item.month == 5)
                            worksheet.Cells[e + 1, 7].Value = item.montant;
                        if (item.month == 6)
                            worksheet.Cells[e + 1, 8].Value = item.montant;
                        if (item.month == 7)
                            worksheet.Cells[e + 1, 9].Value = item.montant;
                        if (item.month == 8)
                            worksheet.Cells[e + 1, 10].Value = item.montant;
                        if (item.month == 9)
                            worksheet.Cells[e + 1, 11].Value = item.montant;
                        if (item.month == 10)
                            worksheet.Cells[e + 1, 12].Value = item.montant;
                        if (item.month == 11)
                            worksheet.Cells[e + 1, 13].Value = item.montant;
                        if (item.month == 12)
                            worksheet.Cells[e + 1, 14].Value = item.montant;
                    }
                    worksheet.Cells[e + 1, 15].Formula = "=B" + range + "/" + list.Count;
                    if (listImputEntree.Keys.Count == 1)
                    {
                        range += 1;
                        worksheet.Cells[e + 2, 15].Formula = "=B" + range + "/" + list.Count;
                    }
                }
                else
                {
                    tabRec.AddRow(1);
                    worksheet.Cells[e + 1, 1].Value = key;
                    int range = e + 1;
                    worksheet.Cells[e + 1, 2].Formula = "=SUM(C" + range + ":N" + range + ")";
                    foreach (var item in list)
                    {
                        if (item.month == 1)
                            worksheet.Cells[e + 1, 3].Value = item.montant;
                        if (item.month == 2)
                            worksheet.Cells[e + 1, 4].Value = item.montant;
                        if (item.month == 3)
                            worksheet.Cells[e + 1, 5].Value = item.montant;
                        if (item.month == 4)
                            worksheet.Cells[e + 1, 6].Value = item.montant;
                        if (item.month == 5)
                            worksheet.Cells[e + 1, 7].Value = item.montant;
                        if (item.month == 6)
                            worksheet.Cells[e + 1, 8].Value = item.montant;
                        if (item.month == 7)
                            worksheet.Cells[e + 1, 9].Value = item.montant;
                        if (item.month == 8)
                            worksheet.Cells[e + 1, 10].Value = item.montant;
                        if (item.month == 9)
                            worksheet.Cells[e + 1, 11].Value = item.montant;
                        if (item.month == 10)
                            worksheet.Cells[e + 1, 12].Value = item.montant;
                        if (item.month == 11)
                            worksheet.Cells[e + 1, 13].Value = item.montant;
                        if (item.month == 12)
                            worksheet.Cells[e + 1, 14].Value = item.montant;
                    }
                    worksheet.Cells[e + 1, 15].Formula = "=B" + range + "/" + list.Count;
                    range += 2;
                    worksheet.Cells[e + 2, 15].Formula = "=B" + range + "/" + list.Count;
                }
                e++;
            }

            e = listImputEntree.Keys.Count + 8;
            //ADDING DEPENSES TO EXCEL FILE
            foreach (string key in listImputSortie.Keys)
            {
                List<ListMonthInYear> list = listImputSortie[key];
                if (e == listImputEntree.Keys.Count + 8)
                {
                    worksheet.Cells[e + 1, 1].Value = key;
                    int range = e + 1;
                    worksheet.Cells[e + 1, 2].Formula = "=SUM(C" + range + ":N" + range + ")";

                    foreach (var item in list)
                    {
                        if (item.month == 1)
                            worksheet.Cells[e + 1, 3].Value = item.montant;
                        if (item.month == 2)
                            worksheet.Cells[e + 1, 4].Value = item.montant;
                        if (item.month == 3)
                            worksheet.Cells[e + 1, 5].Value = item.montant;
                        if (item.month == 4)
                            worksheet.Cells[e + 1, 6].Value = item.montant;
                        if (item.month == 5)
                            worksheet.Cells[e + 1, 7].Value = item.montant;
                        if (item.month == 6)
                            worksheet.Cells[e + 1, 8].Value = item.montant;
                        if (item.month == 7)
                            worksheet.Cells[e + 1, 9].Value = item.montant;
                        if (item.month == 8)
                            worksheet.Cells[e + 1, 10].Value = item.montant;
                        if (item.month == 9)
                            worksheet.Cells[e + 1, 11].Value = item.montant;
                        if (item.month == 10)
                            worksheet.Cells[e + 1, 12].Value = item.montant;
                        if (item.month == 11)
                            worksheet.Cells[e + 1, 13].Value = item.montant;
                        if (item.month == 12)
                            worksheet.Cells[e + 1, 14].Value = item.montant;
                    }
                    worksheet.Cells[e + 1, 15].Formula = "=B" + range + "/" + list.Count;
                    if (listImputSortie.Keys.Count == 1)
                    {
                        range += 1;
                        worksheet.Cells[e + 2, 15].Formula = "=B" + range + "/" + list.Count;
                    }

                }
                else
                {
                    tabDep.AddRow(1);
                    worksheet.Cells[e + 1, 1].Value = key;
                    int range = e + 1;
                    worksheet.Cells[e + 1, 2].Formula = "=SUM(C" + range + ":N" + range + ")";
                    foreach (var item in list)
                    {
                        if (item.month == 1)
                            worksheet.Cells[e + 1, 3].Value = item.montant;
                        if (item.month == 2)
                            worksheet.Cells[e + 1, 4].Value = item.montant;
                        if (item.month == 3)
                            worksheet.Cells[e + 1, 5].Value = item.montant;
                        if (item.month == 4)
                            worksheet.Cells[e + 1, 6].Value = item.montant;
                        if (item.month == 5)
                            worksheet.Cells[e + 1, 7].Value = item.montant;
                        if (item.month == 6)
                            worksheet.Cells[e + 1, 8].Value = item.montant;
                        if (item.month == 7)
                            worksheet.Cells[e + 1, 9].Value = item.montant;
                        if (item.month == 8)
                            worksheet.Cells[e + 1, 10].Value = item.montant;
                        if (item.month == 9)
                            worksheet.Cells[e + 1, 11].Value = item.montant;
                        if (item.month == 10)
                            worksheet.Cells[e + 1, 12].Value = item.montant;
                        if (item.month == 11)
                            worksheet.Cells[e + 1, 13].Value = item.montant;
                        if (item.month == 12)
                            worksheet.Cells[e + 1, 14].Value = item.montant;
                    }
                    worksheet.Cells[e + 1, 15].Formula = "=B" + range + "/" + list.Count;
                    range += 1;
                    worksheet.Cells[e + 2, 15].Formula = "=B" + range + "/" + list.Count;
                }
                e++;
            }

            return package;
        }

        public class ListMonthInYear
        {
            //public string imputation { get; set; }
            public int month { get; set; }
            public string type { get; set; }
            public double montant { get; set; }
        }
    }
}
