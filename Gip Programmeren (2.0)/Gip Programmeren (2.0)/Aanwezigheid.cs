using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIP_Programmeren
{
    class Aanwezigheid
    {
        String _aankomsttijd;
        String _statusid;
        String _idleerling;
        String _aanwezigheidid;

        public String dtAankomstTijd
        {
            get { return _aankomsttijd; }
            set { _aankomsttijd = value; }
        }

        public string strStatusId
        {
            get { return _statusid; }
            set { _statusid = value; }
        }

        public string strLeerlingid
        {
            get { return _idleerling; }
            set { _idleerling = value; }
        }

        public string strAanwezigheidId
        {
            get { return _aanwezigheidid; }
            set { _aanwezigheidid = value; }
        }

        public override string ToString()
        {
            return _aankomsttijd + " " + _statusid;
        }

        public Aanwezigheid(String aanwezigheidid, String aankomsttijd, String idleerling, String statusid)
        {
            _aankomsttijd = aankomsttijd;
            _statusid = statusid;
            _idleerling = idleerling;
            _aanwezigheidid = aanwezigheidid;
        }

        public Aanwezigheid()
        {

        }
    }
}
