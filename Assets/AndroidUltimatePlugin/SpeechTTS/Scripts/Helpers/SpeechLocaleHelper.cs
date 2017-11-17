using UnityEngine;
using System.Collections;

public class SpeechLocaleHelper : MonoBehaviour {

	#region getTTSLocale
	public static TTSLocaleCountry GetTTSExtraLocale(SpeechExtraLocale speechExtraLocale){

		if(speechExtraLocale == SpeechExtraLocale.MX){
			return TTSLocaleCountry.MEXICO;
		}else if(speechExtraLocale == SpeechExtraLocale.ES){
			return TTSLocaleCountry.SPAIN;
		}else if(speechExtraLocale == SpeechExtraLocale.CO){
			return TTSLocaleCountry.COLOMBIA;
		}else if(speechExtraLocale == SpeechExtraLocale.AR){
			return TTSLocaleCountry.ARGENTINA;
		}else if(speechExtraLocale == SpeechExtraLocale.PE){
			return TTSLocaleCountry.PERU;
		}else if(speechExtraLocale == SpeechExtraLocale.VE){
			return TTSLocaleCountry.VENEZUELA;
		}else if(speechExtraLocale == SpeechExtraLocale.CL){
			return TTSLocaleCountry.CHILE;
		}else if(speechExtraLocale == SpeechExtraLocale.GT){
			return TTSLocaleCountry.GUATEMALA;
		}else if(speechExtraLocale == SpeechExtraLocale.AE){
			return TTSLocaleCountry.UNITED_ARAB_EMIRATES;
		}else if(speechExtraLocale == SpeechExtraLocale.AU){
			return TTSLocaleCountry.AUSTRALIA;
		}else if(speechExtraLocale == SpeechExtraLocale.BG){
			return TTSLocaleCountry.BULGARIA;
		}else if(speechExtraLocale == SpeechExtraLocale.BH){
			return TTSLocaleCountry.BAHRAIN;
		}else if(speechExtraLocale == SpeechExtraLocale.CO){
			return TTSLocaleCountry.COLOMBIA;
		}else if(speechExtraLocale == SpeechExtraLocale.CR){
			return TTSLocaleCountry.COSTA_RICA;
		}else if(speechExtraLocale == SpeechExtraLocale.DK){
			return TTSLocaleCountry.DENMARK;
		}else if(speechExtraLocale == SpeechExtraLocale.DO){
			return TTSLocaleCountry.DOMINICAN_REPUBLIC;
		}else if(speechExtraLocale == SpeechExtraLocale.DZ){
			return TTSLocaleCountry.ALGERIA;
		}else if(speechExtraLocale == SpeechExtraLocale.EG){
			return TTSLocaleCountry.EGYPT;
		}else if(speechExtraLocale == SpeechExtraLocale.ENZA){
			return TTSLocaleCountry.SOUTH_AFRICA;
		}else if(speechExtraLocale == SpeechExtraLocale.ESES){
			return TTSLocaleCountry.SPAIN;
		}else if(speechExtraLocale == SpeechExtraLocale.FI){
			return TTSLocaleCountry.FINLAND;
		}else if(speechExtraLocale == SpeechExtraLocale.FILPH){
			return TTSLocaleCountry.PHILIPPINES;
		}else if(speechExtraLocale == SpeechExtraLocale.GB){
			return TTSLocaleCountry.UNITED_KINGDOM;
		}else if(speechExtraLocale == SpeechExtraLocale.GLES){
			return TTSLocaleCountry.SPAIN;
		}else if(speechExtraLocale == SpeechExtraLocale.GR){
			return TTSLocaleCountry.GREECE;
		}else if(speechExtraLocale == SpeechExtraLocale.HIIN){
			return TTSLocaleCountry.INDIA;
		}else if(speechExtraLocale == SpeechExtraLocale.HK){
			return TTSLocaleCountry.HONGKONG;
		}else if(speechExtraLocale == SpeechExtraLocale.HN){
			return TTSLocaleCountry.HONDURAS;
		}else if(speechExtraLocale == SpeechExtraLocale.HR){
			return TTSLocaleCountry.CROATIA;
		}else if(speechExtraLocale == SpeechExtraLocale.HU){
			return TTSLocaleCountry.HUNGARY;
		}else if(speechExtraLocale == SpeechExtraLocale.ID){
			return TTSLocaleCountry.INDONESIA;
		}else if(speechExtraLocale == SpeechExtraLocale.IE){
			return TTSLocaleCountry.IRELAND;
		}else if(speechExtraLocale == SpeechExtraLocale.IL){
			return TTSLocaleCountry.ISRAEL;
		}else if(speechExtraLocale == SpeechExtraLocale.IN){
			return TTSLocaleCountry.INDIA;
		}else if(speechExtraLocale == SpeechExtraLocale.IR){
			return TTSLocaleCountry.IRAN;
		}else if(speechExtraLocale == SpeechExtraLocale.IS){
			return TTSLocaleCountry.ICELAND;
		}else if(speechExtraLocale == SpeechExtraLocale.JO){
			return TTSLocaleCountry.JORDAN;
		}else if(speechExtraLocale == SpeechExtraLocale.KW){
			return TTSLocaleCountry.KUWAIT;
		}else if(speechExtraLocale == SpeechExtraLocale.LB){
			return TTSLocaleCountry.LEBANON;
		}else if(speechExtraLocale == SpeechExtraLocale.LT){
			return TTSLocaleCountry.LITHUANIA;
		}else if(speechExtraLocale == SpeechExtraLocale.MA){
			return TTSLocaleCountry.MOROCCO;
		}else if(speechExtraLocale == SpeechExtraLocale.MY){
			return TTSLocaleCountry.MALAYSIA;
		}else if(speechExtraLocale == SpeechExtraLocale.NI){
			return TTSLocaleCountry.NICARAGUA;
		}else if(speechExtraLocale == SpeechExtraLocale.NL){
			return TTSLocaleCountry.NETHERLANDS;
		}else if(speechExtraLocale == SpeechExtraLocale.NZ){
			return TTSLocaleCountry.NEW_ZEALAND;
		}else if(speechExtraLocale == SpeechExtraLocale.OM){
			return TTSLocaleCountry.OMAN;
		}else if(speechExtraLocale == SpeechExtraLocale.PA){
			return TTSLocaleCountry.PANAMA;
		}else if(speechExtraLocale == SpeechExtraLocale.PE){
			return TTSLocaleCountry.PERU;
		}else if(speechExtraLocale == SpeechExtraLocale.PH){
			return TTSLocaleCountry.PHILIPPINES;
		}else if(speechExtraLocale == SpeechExtraLocale.PL){
			return TTSLocaleCountry.POLAND;
		}else if(speechExtraLocale == SpeechExtraLocale.PR){
			return TTSLocaleCountry.PUERTO_RICO;
		}else if(speechExtraLocale == SpeechExtraLocale.PTBR){
			return TTSLocaleCountry.BRAZIL;
		}else if(speechExtraLocale == SpeechExtraLocale.PY){
			return TTSLocaleCountry.PARAGUAY;
		}else if(speechExtraLocale == SpeechExtraLocale.QA){
			return TTSLocaleCountry.QATAR;
		}else if(speechExtraLocale == SpeechExtraLocale.RO){
			return TTSLocaleCountry.ROMANIA;
		}else if(speechExtraLocale == SpeechExtraLocale.RS){
			return TTSLocaleCountry.SERBIA;
		}else if(speechExtraLocale == SpeechExtraLocale.RU){
			return TTSLocaleCountry.RUSSIAN_FEDERATION;
		}else if(speechExtraLocale == SpeechExtraLocale.SA){
			return TTSLocaleCountry.SAUDI_ARABIA;
		}else if(speechExtraLocale == SpeechExtraLocale.SE){
			return TTSLocaleCountry.SWEDEN;
		}else if(speechExtraLocale == SpeechExtraLocale.SI){
			return TTSLocaleCountry.SLOVENIA;
		}else if(speechExtraLocale == SpeechExtraLocale.SK){
			return TTSLocaleCountry.SLOVAKIA;
		}else if(speechExtraLocale == SpeechExtraLocale.SV){
			return TTSLocaleCountry.EL_SALVADOR;
		}else if(speechExtraLocale == SpeechExtraLocale.TH){
			return TTSLocaleCountry.THAILAND;
		}else if(speechExtraLocale == SpeechExtraLocale.TN){
			return TTSLocaleCountry.TUNISIA;
		}else if(speechExtraLocale == SpeechExtraLocale.TR){
			return TTSLocaleCountry.TURKEY;
		}else if(speechExtraLocale == SpeechExtraLocale.UA){
			return TTSLocaleCountry.UKRAINE;
		}else if(speechExtraLocale == SpeechExtraLocale.UEES){
			return TTSLocaleCountry.SPAIN;
		}else if(speechExtraLocale == SpeechExtraLocale.UY){
			return TTSLocaleCountry.URUGUAY;
		}else if(speechExtraLocale == SpeechExtraLocale.VN){
			return TTSLocaleCountry.VIETNAM;
		}else if(speechExtraLocale == SpeechExtraLocale.YUEHANTHK){
			return TTSLocaleCountry.HONGKONG;
		}else if(speechExtraLocale == SpeechExtraLocale.ZA){
			return TTSLocaleCountry.SOUTH_AFRICA;
		}else if(speechExtraLocale == SpeechExtraLocale.ZUZA){
			return TTSLocaleCountry.SOUTH_AFRICA;
		}else{
			return TTSLocaleCountry.UNITED_STATES;
		}
	}
	#endregion

	public static SpeechLocale GetSpeechLocale(SpeechExtraLocale speechExtraLocale){
		if(speechExtraLocale == SpeechExtraLocale.US){
			return SpeechLocale.US;
		}else if(speechExtraLocale == SpeechExtraLocale.JP){
			return SpeechLocale.JAPANESE;
		}else if(speechExtraLocale == SpeechExtraLocale.FR){
			return SpeechLocale.FRANCE;
		}else if(speechExtraLocale == SpeechExtraLocale.IT){
			return SpeechLocale.ITALIAN;
		}else if(speechExtraLocale == SpeechExtraLocale.KR){
			return SpeechLocale.KOREAN;
		}else if(speechExtraLocale == SpeechExtraLocale.TW){
			return SpeechLocale.TAIWAN;
		}else if(speechExtraLocale == SpeechExtraLocale.CN){
			return SpeechLocale.CHINESE;
		}else if(speechExtraLocale == SpeechExtraLocale.CA){
			return SpeechLocale.CANADA;
		}else if(speechExtraLocale == SpeechExtraLocale.CN){
			return SpeechLocale.CHINA;
		}else if(speechExtraLocale == SpeechExtraLocale.ESUS){
			return SpeechLocale.US;
		}else{
			return SpeechLocale.NONE;
		}
	}
}
