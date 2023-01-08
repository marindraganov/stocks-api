namespace StocksAPI.Data
{
    public class StocksIDMapping
    {
        private static List<string> stocks = new List<string>() { "1-AAPL", "2-MSFT", "3-AMZN", "4-BRK.B", "5-GOOGL", "6-UNH", "7-GOOG", "8-JNJ", "9-XOM", "10-JPM", "11-NVDA", "12-PG", "13-V", "14-HD", "15-TSLA", "16-CVX", "17-MA", "18-LLY", "19-PFE", "20-ABBV", "21-MRK", "22-META", "23-PEP", "24-KO", "25-BAC", "26-AVGO", "27-TMO", "28-COST", "29-WMT", "30-CSCO", "31-MCD", "32-ABT", "33-DHR", "34-ACN", "35-NEE", "36-VZ", "37-LIN", "38-DIS", "39-WFC", "40-ADBE", "41-PM", "42-BMY", "43-CMCSA", "44-TXN", "45-NKE", "46-RTX", "47-COP", "48-HON", "49-AMGN", "50-CRM", "51-T", "52-NFLX", "53-UNP", "54-UPS", "55-IBM", "56-SCHW", "57-LOW", "58-ORCL", "59-CAT", "60-QCOM", "61-CVS", "62-ELV", "63-DE", "64-GS", "65-SBUX", "66-LMT", "67-SPGI", "68-MS", "69-INTU", "70-INTC", "71-BLK", "72-GILD", "73-BA", "74-PLD", "75-AMD", "76-MDT", "77-CI", "78-AMT", "79-ADP", "80-ISRG", "81-TJX", "82-CB", "83-MDLZ", "84-GE", "85-AXP", "86-C", "87-ADI", "88-TMUS", "89-AMAT", "90-MMC", "91-SYK", "92-MO", "93-PYPL", "94-DUK", "95-SO", "96-NOW", "97-NOC", "98-BKNG", "99-REGN", "100-PGR", "101-EOG", "102-SLB", "103-VRTX", "104-BDX", "105-APD", "106-ZTS", "107-TGT", "108-MMM", "109-BSX", "110-CL", "111-CSX", "112-HUM", "113-FISV", "114-PNC", "115-AON", "116-ETN", "117-ITW", "118-EQIX", "119-CME", "120-CCI", "121-WM", "122-MRNA", "123-USB", "124-ICE", "125-EL", "126-NSC", "127-LRCX", "128-TFC", "129-EMR", "130-SHW", "131-DG", "132-GD", "133-MU", "134-FCX", "135-ATVI", "136-MPC", "137-PXD", "138-MCK", "139-KLAC", "140-ORLY", "141-HCA", "142-ADM", "143-D", "144-GIS", "145-PSX", "146-AEP", "147-SRE", "148-SNPS", "149-VLO", "150-MET", "151-GM", "152-AIG", "153-AZO", "154-CNC", "155-EW", "156-ROP", "157-KMB", "158-APH", "159-F", "160-OXY", "161-A", "162-TRV", "163-PSA", "164-MCO", "165-JCI", "166-CDNS", "167-DXCM", "168-MSI", "169-EXC", "170-CTVA", "171-FDX", "172-NXPI", "173-AFL", "174-ADSK", "175-ROST", "176-FIS", "177-O", "178-WMB", "179-AJG", "180-BIIB", "181-DVN", "182-MAR", "183-TT", "184-LHX", "185-CTAS", "186-MNST", "187-SYY", "188-XEL", "189-CMG", "190-HES", "191-MCHP", "192-IQV", "193-STZ", "194-SPG", "195-MSCI", "196-NEM", "197-PAYX", "198-PH", "199-PRU", "200-TEL", "201-YUM", "202-ALL", "203-CHTR", "204-ECL", "205-DOW", "206-KMI", "207-ENPH", "208-COF", "209-HAL", "210-CARR", "211-NUE", "212-HLT", "213-PCAR", "214-ED", "215-DD", "216-HSY", "217-IDXX", "218-CMI", "219-AMP", "220-BK", "221-OTIS", "222-MTD", "223-KHC", "224-TDG", "225-EA", "226-FTNT", "227-AME", "228-ILMN", "229-CSGP", "230-WELL", "231-VICI", "232-KEYS", "233-PEG", "234-RMD", "235-SBAC", "236-KDP", "237-WEC", "238-DLTR", "239-ANET", "240-ROK", "241-PPG", "242-OKE", "243-CTSH", "244-BKR", "245-ES", "246-KR", "247-DLR", "248-STT", "249-CEG", "250-DHI" };
        private readonly Dictionary<int, string> _idToSymbol;
        private readonly Dictionary<string, int> _symbolToID;


        public StocksIDMapping() 
        {
            _idToSymbol= new Dictionary<int, string>();
            _symbolToID= new Dictionary<string, int>();

            foreach (var st in stocks)
            {
                var id = int.Parse(st.Split('-')[0]);
                var symbol = st.Split('-')[1];

                _idToSymbol[id] = symbol;
                _symbolToID[symbol] = id;
            }
        }

        public string GetSymbol(int id)
        {
            return _idToSymbol[id];
        }

        public int GetID(string symbol)
        {
            return _symbolToID[symbol];
        }

        public IEnumerable<string> GetAllStockSymbols()
        {
            return _idToSymbol.Values;
        }
    }
}
