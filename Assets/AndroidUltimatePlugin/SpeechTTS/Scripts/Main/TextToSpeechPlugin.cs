using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TextToSpeechPlugin : MonoBehaviour
{
	
    private static TextToSpeechPlugin instance;
    private static GameObject container;
    private const string TAG = "[TextToSpeechPlugin]: ";
    private static AUPHolder aupHolder;

    private Dictionary<string, string> localeCountryDict = new Dictionary<string,string>();
    private Dictionary<string, TTSLocaleCountry> countryISO2AlphaDict = new Dictionary<string,TTSLocaleCountry>();
	
    #if UNITY_ANDROID
    private static AndroidJavaObject jo;
    #endif
	
    public bool isDebug = true;

    //new
    private string[] localeCountryISO2AlphaSet = null;
    private bool hasTTSInit = false;
    private bool hasInit = false;

    private Action <int> Init;

    public event Action <int>OnInit
    {
        add{ Init += value;}
        remove{ Init -= value;}
    }

    private Action <string> GetLocaleCountry;

    public event Action <string>OnGetLocaleCountry
    {
        add{ GetLocaleCountry += value;}
        remove{ GetLocaleCountry -= value;}
    }

    private Action <int> ChangeLocale;

    public event Action <int>OnChangeLocale
    {
        add{ ChangeLocale += value;}
        remove{ ChangeLocale -= value;}
    }

    private Action <string> StartSpeech;

    public event Action <string>OnStartSpeech
    {
        add{ StartSpeech += value;}
        remove{ StartSpeech -= value;}
    }

    private Action <string> EndSpeech;

    public event Action <string>OnEndSpeech
    {
        add{ EndSpeech += value;}
        remove{ EndSpeech -= value;}
    }

    private Action <string> ErrorSpeech;

    public event Action <string>OnErrorSpeech
    {
        add{ ErrorSpeech += value;}
        remove{ ErrorSpeech -= value;}
    }

	
    public static TextToSpeechPlugin GetInstance()
    {
        if (instance == null)
        {
            container = new GameObject();
            container.name = "TextToSpeechPlugin";
            instance = container.AddComponent(typeof(TextToSpeechPlugin)) as TextToSpeechPlugin;
            DontDestroyOnLoad(instance.gameObject);
            aupHolder = AUPHolder.GetInstance();
            instance.gameObject.transform.SetParent(aupHolder.gameObject.transform);
        }
		
        return instance;
    }

    private void Awake()
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo = new AndroidJavaObject("com.gigadrillgames.androidplugin.tts.TextToSpeechPlugin");
        }
        #endif

        LoadLocaleCountry();
        LoadCountryISO2AlphaCountryName();
    }

    /// <summary>
    /// Sets the debug.
    /// 0 - false, 1 - true
    /// </summary>
    /// <param name="debug">Debug.</param>
    public void SetDebug(int debug)
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("SetDebug", debug);
            AUP.Utils.Message(TAG, "SetDebug");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    public Dictionary<string, string> GetLocaleCountryDict()
    {
        return localeCountryDict;
    }

    public Dictionary<string, TTSLocaleCountry> GetCountryISO2AlphaDict()
    {
        return countryISO2AlphaDict;
    }

    #region LoadLocaleCountry

    private void LoadLocaleCountry()
    {
        localeCountryDict.Add(TTSLocaleCountry.AFGHANISTAN.ToString(), "AF");
        localeCountryDict.Add(TTSLocaleCountry.ALAND_ISLANDS.ToString(), "AX");
        localeCountryDict.Add(TTSLocaleCountry.ALBANIA.ToString(), "AL");
        localeCountryDict.Add(TTSLocaleCountry.ALGERIA.ToString(), "DZ");
        localeCountryDict.Add(TTSLocaleCountry.AMERICAN_SAMOA.ToString(), "AS");
        localeCountryDict.Add(TTSLocaleCountry.ANDORRA.ToString(), "AD");
        localeCountryDict.Add(TTSLocaleCountry.ANGOLA.ToString(), "AO");
        localeCountryDict.Add(TTSLocaleCountry.ANGUILLA.ToString(), "AI");
        localeCountryDict.Add(TTSLocaleCountry.ANTARCTICA.ToString(), "AQ");
        localeCountryDict.Add(TTSLocaleCountry.ANTIGUA_AND_BARBUDA.ToString(), "AG");
        localeCountryDict.Add(TTSLocaleCountry.ARGENTINA.ToString(), "AR");
        localeCountryDict.Add(TTSLocaleCountry.ARMENIA.ToString(), "AM");
        localeCountryDict.Add(TTSLocaleCountry.ARUBA.ToString(), "AW");
        localeCountryDict.Add(TTSLocaleCountry.AUSTRALIA.ToString(), "AU");
        localeCountryDict.Add(TTSLocaleCountry.AUSTRIA.ToString(), "AT");
        localeCountryDict.Add(TTSLocaleCountry.AZERBAIJAN.ToString(), "AZ");

        localeCountryDict.Add(TTSLocaleCountry.BAHAMAS.ToString(), "BS");
        localeCountryDict.Add(TTSLocaleCountry.BAHRAIN.ToString(), "BH");
        localeCountryDict.Add(TTSLocaleCountry.BANGLADESH.ToString(), "BD");
        localeCountryDict.Add(TTSLocaleCountry.BARBADOS.ToString(), "BB");
        localeCountryDict.Add(TTSLocaleCountry.BELARUS.ToString(), "BY");
        localeCountryDict.Add(TTSLocaleCountry.BELGIUM.ToString(), "BE");
        localeCountryDict.Add(TTSLocaleCountry.BELIZE.ToString(), "BZ");
        localeCountryDict.Add(TTSLocaleCountry.BENIN.ToString(), "BJ");
        localeCountryDict.Add(TTSLocaleCountry.BERMUDA.ToString(), "BM");
        localeCountryDict.Add(TTSLocaleCountry.BHUTAN.ToString(), "BT");
        localeCountryDict.Add(TTSLocaleCountry.BOLIVIA.ToString(), "BO");
        localeCountryDict.Add(TTSLocaleCountry.BONAIRE_ST_EUSTATIUS_AND_SABA.ToString(), "BQ");
        localeCountryDict.Add(TTSLocaleCountry.BOSNIA_AND_HERZEGOVINA.ToString(), "BA");
        localeCountryDict.Add(TTSLocaleCountry.BOTSWANA.ToString(), "BW");
        localeCountryDict.Add(TTSLocaleCountry.BOUVET_ISLAND.ToString(), "BV");
        localeCountryDict.Add(TTSLocaleCountry.BRAZIL.ToString(), "BR");

        localeCountryDict.Add(TTSLocaleCountry.BRITISH_INDIAN_OCEAN_TERRITORY.ToString(), "IO");
        localeCountryDict.Add(TTSLocaleCountry.BRUNEI_DARUSSALAM.ToString(), "BN");
        localeCountryDict.Add(TTSLocaleCountry.BULGARIA.ToString(), "BG");
        localeCountryDict.Add(TTSLocaleCountry.BURKINA_FASO.ToString(), "BF");
        localeCountryDict.Add(TTSLocaleCountry.BURUNDI.ToString(), "BI");
        localeCountryDict.Add(TTSLocaleCountry.CAMBODIA.ToString(), "KH");
        localeCountryDict.Add(TTSLocaleCountry.CAMEROON.ToString(), "CM");
        localeCountryDict.Add(TTSLocaleCountry.CANADA.ToString(), "CA");
        localeCountryDict.Add(TTSLocaleCountry.CAPE_VERDE.ToString(), "CV");
        localeCountryDict.Add(TTSLocaleCountry.CAYMAN_ISLANDS.ToString(), "KY");
        localeCountryDict.Add(TTSLocaleCountry.CENTRAL_AFRICAN_REPUBLIC.ToString(), "CF");
        localeCountryDict.Add(TTSLocaleCountry.CHAD.ToString(), "TD");
        localeCountryDict.Add(TTSLocaleCountry.CHILE.ToString(), "CL");
        localeCountryDict.Add(TTSLocaleCountry.CHINA.ToString(), "CN");

        localeCountryDict.Add(TTSLocaleCountry.CHRISTMAS_ISLAND.ToString(), "CX");
        localeCountryDict.Add(TTSLocaleCountry.COCOS_ISLANDS.ToString(), "CC");
        localeCountryDict.Add(TTSLocaleCountry.COLOMBIA.ToString(), "CO");
        localeCountryDict.Add(TTSLocaleCountry.COMOROS.ToString(), "KM");
        localeCountryDict.Add(TTSLocaleCountry.CONGO.ToString(), "CG");
        localeCountryDict.Add(TTSLocaleCountry.COOK_ISLANDS.ToString(), "CK");
        localeCountryDict.Add(TTSLocaleCountry.COSTA_RICA.ToString(), "CR");
        localeCountryDict.Add(TTSLocaleCountry.COTE_DIVOIRE.ToString(), "CI");
        localeCountryDict.Add(TTSLocaleCountry.CROATIA.ToString(), "HR");
        localeCountryDict.Add(TTSLocaleCountry.CUBA.ToString(), "CU");
        localeCountryDict.Add(TTSLocaleCountry.CURACAO.ToString(), "CW");
        localeCountryDict.Add(TTSLocaleCountry.CYPRUS.ToString(), "CY");
        localeCountryDict.Add(TTSLocaleCountry.CZECH_REPUBLIC.ToString(), "CZ");
        localeCountryDict.Add(TTSLocaleCountry.DENMARK.ToString(), "DK");
        localeCountryDict.Add(TTSLocaleCountry.DJIBOUTI.ToString(), "DJ");

        localeCountryDict.Add(TTSLocaleCountry.DOMINICA.ToString(), "DM");
        localeCountryDict.Add(TTSLocaleCountry.DOMINICAN_REPUBLIC.ToString(), "DO");
        localeCountryDict.Add(TTSLocaleCountry.ECUADOR.ToString(), "EC");
        localeCountryDict.Add(TTSLocaleCountry.EL_SALVADOR.ToString(), "SV");
        localeCountryDict.Add(TTSLocaleCountry.EQUATORIAL_GUINEA.ToString(), "GQ");
        localeCountryDict.Add(TTSLocaleCountry.ERITREA.ToString(), "ER");
        localeCountryDict.Add(TTSLocaleCountry.ESTONIA.ToString(), "EE");
        localeCountryDict.Add(TTSLocaleCountry.ETHIOPIA.ToString(), "ET");
        localeCountryDict.Add(TTSLocaleCountry.FAEROE_ISLANDS.ToString(), "FO");
        localeCountryDict.Add(TTSLocaleCountry.FALKLAND_ISLANDS.ToString(), "FK");
        localeCountryDict.Add(TTSLocaleCountry.FIJI.ToString(), "FJ");
        localeCountryDict.Add(TTSLocaleCountry.FINLAND.ToString(), "FI");
        localeCountryDict.Add(TTSLocaleCountry.FRANCE.ToString(), "FR");

        localeCountryDict.Add(TTSLocaleCountry.FRENCH_GUIANA.ToString(), "GF");
        localeCountryDict.Add(TTSLocaleCountry.FRENCH_POLYNESIA.ToString(), "PF");
        localeCountryDict.Add(TTSLocaleCountry.FRENCH_SOUTHERN_TERRITORIES.ToString(), "TF");
        localeCountryDict.Add(TTSLocaleCountry.GABON.ToString(), "GA");
        localeCountryDict.Add(TTSLocaleCountry.GAMBIA_THE.ToString(), "GM");
        localeCountryDict.Add(TTSLocaleCountry.GHANA.ToString(), "GH");
        localeCountryDict.Add(TTSLocaleCountry.GIBRALTAR.ToString(), "GI");
        localeCountryDict.Add(TTSLocaleCountry.GREECE.ToString(), "GR");
        localeCountryDict.Add(TTSLocaleCountry.GREENLAND.ToString(), "GL");
        localeCountryDict.Add(TTSLocaleCountry.GRENADA.ToString(), "GD");
        localeCountryDict.Add(TTSLocaleCountry.GUADELOUPE.ToString(), "GP");
        localeCountryDict.Add(TTSLocaleCountry.GUAM.ToString(), "GU");
        localeCountryDict.Add(TTSLocaleCountry.GUATEMALA.ToString(), "GT");

        localeCountryDict.Add(TTSLocaleCountry.GUERNSEY.ToString(), "GG");
        localeCountryDict.Add(TTSLocaleCountry.GUINEA.ToString(), "GN");
        localeCountryDict.Add(TTSLocaleCountry.GUINEA_BISSAU.ToString(), "GW");
        localeCountryDict.Add(TTSLocaleCountry.GUYANA.ToString(), "GY");
        localeCountryDict.Add(TTSLocaleCountry.HAITI.ToString(), "HT");
        localeCountryDict.Add(TTSLocaleCountry.HEARD_ISLAND_AND_MCDONALD_ISLANDS.ToString(), "HM");
        localeCountryDict.Add(TTSLocaleCountry.HONDURAS.ToString(), "HN");
        localeCountryDict.Add(TTSLocaleCountry.HONGKONG.ToString(), "HK");
        localeCountryDict.Add(TTSLocaleCountry.HUNGARY.ToString(), "HU");
        localeCountryDict.Add(TTSLocaleCountry.ICELAND.ToString(), "IS");
        localeCountryDict.Add(TTSLocaleCountry.INDIA.ToString(), "IN");
        localeCountryDict.Add(TTSLocaleCountry.INDONESIA.ToString(), "ID");
        localeCountryDict.Add(TTSLocaleCountry.IRAN.ToString(), "IR");
        localeCountryDict.Add(TTSLocaleCountry.IRAQ.ToString(), "IQ");
        localeCountryDict.Add(TTSLocaleCountry.IRELAND.ToString(), "IE");

        localeCountryDict.Add(TTSLocaleCountry.ISLE_OF_MAN.ToString(), "IM");
        localeCountryDict.Add(TTSLocaleCountry.ISRAEL.ToString(), "IL");
        localeCountryDict.Add(TTSLocaleCountry.ITALY.ToString(), "IT");
        localeCountryDict.Add(TTSLocaleCountry.JAMAICA.ToString(), "JM");
        localeCountryDict.Add(TTSLocaleCountry.JAPAN.ToString(), "JP");
        localeCountryDict.Add(TTSLocaleCountry.JERSEY.ToString(), "JE");
        localeCountryDict.Add(TTSLocaleCountry.JORDAN.ToString(), "JO");
        localeCountryDict.Add(TTSLocaleCountry.KAZAKHSTAN.ToString(), "KZ");
        localeCountryDict.Add(TTSLocaleCountry.KENYA.ToString(), "KE");
        localeCountryDict.Add(TTSLocaleCountry.KIRIBATI.ToString(), "KI");
        localeCountryDict.Add(TTSLocaleCountry.NORTH_KOREA.ToString(), "KP");
        localeCountryDict.Add(TTSLocaleCountry.SOUTH_KOREA.ToString(), "KR");
        localeCountryDict.Add(TTSLocaleCountry.KUWAIT.ToString(), "KW");
        localeCountryDict.Add(TTSLocaleCountry.KYRGYZSTAN.ToString(), "KG");
        localeCountryDict.Add(TTSLocaleCountry.LAO_PEOPLES_DEMOCRATIC_REPUBLIC.ToString(), "LA");
        localeCountryDict.Add(TTSLocaleCountry.LATVIA.ToString(), "LV");

        localeCountryDict.Add(TTSLocaleCountry.LEBANON.ToString(), "LB");
        localeCountryDict.Add(TTSLocaleCountry.LESOTHO.ToString(), "LS");
        localeCountryDict.Add(TTSLocaleCountry.LIBERIA.ToString(), "LR");
        localeCountryDict.Add(TTSLocaleCountry.LIBYA.ToString(), "LY");
        localeCountryDict.Add(TTSLocaleCountry.LIECHTENSTEIN.ToString(), "LI");
        localeCountryDict.Add(TTSLocaleCountry.LITHUANIA.ToString(), "LT");
        localeCountryDict.Add(TTSLocaleCountry.LUXEMBOURG.ToString(), "LU");
        localeCountryDict.Add(TTSLocaleCountry.MACAO.ToString(), "MO");
        localeCountryDict.Add(TTSLocaleCountry.MACEDONIA.ToString(), "MK");
        localeCountryDict.Add(TTSLocaleCountry.MADAGASCAR.ToString(), "MG");
        localeCountryDict.Add(TTSLocaleCountry.MALAWI.ToString(), "MW");
        localeCountryDict.Add(TTSLocaleCountry.MALDIVES.ToString(), "MV");
        localeCountryDict.Add(TTSLocaleCountry.MALI.ToString(), "ML");
        localeCountryDict.Add(TTSLocaleCountry.MALTA.ToString(), "MT");

        localeCountryDict.Add(TTSLocaleCountry.MARSHALL_ISLANDS.ToString(), "MH");
        localeCountryDict.Add(TTSLocaleCountry.MARTINIQUE.ToString(), "MQ");
        localeCountryDict.Add(TTSLocaleCountry.MAURITANIA.ToString(), "MR");
        localeCountryDict.Add(TTSLocaleCountry.MAURITIUS.ToString(), "MU");
        localeCountryDict.Add(TTSLocaleCountry.MAYOTTE.ToString(), "YT");
        localeCountryDict.Add(TTSLocaleCountry.MEXICO.ToString(), "MX");
        localeCountryDict.Add(TTSLocaleCountry.MICRONESIA.ToString(), "FM");
        localeCountryDict.Add(TTSLocaleCountry.MOLDOVA.ToString(), "MD");
        localeCountryDict.Add(TTSLocaleCountry.MONACO.ToString(), "MC");
        localeCountryDict.Add(TTSLocaleCountry.MONGOLIA.ToString(), "MN");
        localeCountryDict.Add(TTSLocaleCountry.MONTENEGRO.ToString(), "ME");
        localeCountryDict.Add(TTSLocaleCountry.MONTSERRAT.ToString(), "MS");
        localeCountryDict.Add(TTSLocaleCountry.MOROCCO.ToString(), "MA");
        localeCountryDict.Add(TTSLocaleCountry.MOZAMBIQUE.ToString(), "MZ");
        localeCountryDict.Add(TTSLocaleCountry.MYANMAR.ToString(), "MM");
        localeCountryDict.Add(TTSLocaleCountry.NAMIBIA.ToString(), "NA");

        localeCountryDict.Add(TTSLocaleCountry.NAURU.ToString(), "NR");
        localeCountryDict.Add(TTSLocaleCountry.NEPAL.ToString(), "NP");
        localeCountryDict.Add(TTSLocaleCountry.NETHERLANDS.ToString(), "NL");
        localeCountryDict.Add(TTSLocaleCountry.NETHERLANDS_ANTILLES.ToString(), "AN");
        localeCountryDict.Add(TTSLocaleCountry.NEW_CALEDONIA.ToString(), "NC");
        localeCountryDict.Add(TTSLocaleCountry.NEW_ZEALAND.ToString(), "NZ");
        localeCountryDict.Add(TTSLocaleCountry.NICARAGUA.ToString(), "NI");
        localeCountryDict.Add(TTSLocaleCountry.NIGER.ToString(), "NE");
        localeCountryDict.Add(TTSLocaleCountry.NIUE.ToString(), "NU");
        localeCountryDict.Add(TTSLocaleCountry.NORFOLK_ISLAND.ToString(), "NF");
        localeCountryDict.Add(TTSLocaleCountry.NORTHERN_MARIANA_ISLANDS.ToString(), "MP");
        localeCountryDict.Add(TTSLocaleCountry.NORWAY.ToString(), "NO");
        localeCountryDict.Add(TTSLocaleCountry.OMAN.ToString(), "OM");
        localeCountryDict.Add(TTSLocaleCountry.PAKISTAN.ToString(), "PK");

        localeCountryDict.Add(TTSLocaleCountry.PALAU.ToString(), "PW");
        localeCountryDict.Add(TTSLocaleCountry.PALESTINIAN_TERRITORIES.ToString(), "PS");
        localeCountryDict.Add(TTSLocaleCountry.PANAMA.ToString(), "PA");
        localeCountryDict.Add(TTSLocaleCountry.PAPUA_NEW_GUINEA.ToString(), "PG");
        localeCountryDict.Add(TTSLocaleCountry.PARAGUAY.ToString(), "PY");
        localeCountryDict.Add(TTSLocaleCountry.PERU.ToString(), "PE");
        localeCountryDict.Add(TTSLocaleCountry.PHILIPPINES.ToString(), "PH");
        localeCountryDict.Add(TTSLocaleCountry.PITCAIRN.ToString(), "PN");
        localeCountryDict.Add(TTSLocaleCountry.POLAND.ToString(), "PL");
        localeCountryDict.Add(TTSLocaleCountry.PORTUGAL.ToString(), "PT");
        localeCountryDict.Add(TTSLocaleCountry.PUERTO_RICO.ToString(), "PR");
        localeCountryDict.Add(TTSLocaleCountry.QATAR.ToString(), "QA");
        localeCountryDict.Add(TTSLocaleCountry.REUNION.ToString(), "RE");
        localeCountryDict.Add(TTSLocaleCountry.ROMANIA.ToString(), "RO");
        localeCountryDict.Add(TTSLocaleCountry.RUSSIAN_FEDERATION.ToString(), "RU");

        localeCountryDict.Add(TTSLocaleCountry.RWANDA.ToString(), "RW");
        localeCountryDict.Add(TTSLocaleCountry.SAINT_BARTHELEMY.ToString(), "BL");
        localeCountryDict.Add(TTSLocaleCountry.SAINT_HELENA.ToString(), "SH");
        localeCountryDict.Add(TTSLocaleCountry.SAINT_KITTS_AND_NEVIS.ToString(), "KN");
        localeCountryDict.Add(TTSLocaleCountry.SAINT_LUCIA.ToString(), "LC");
        localeCountryDict.Add(TTSLocaleCountry.SAINT_MARTIN.ToString(), "MF");
        localeCountryDict.Add(TTSLocaleCountry.SAINT_PIERRE_AND_MIQUELON.ToString(), "PM");
        localeCountryDict.Add(TTSLocaleCountry.SAINT_VINCENT_AND_THE_GRENADINES.ToString(), "VC");
        localeCountryDict.Add(TTSLocaleCountry.SAMOA.ToString(), "WS");
        localeCountryDict.Add(TTSLocaleCountry.SAN_MARINO.ToString(), "SM");
        localeCountryDict.Add(TTSLocaleCountry.SAO_TOME_AND_PRINCIPE.ToString(), "ST");
        localeCountryDict.Add(TTSLocaleCountry.SAUDI_ARABIA.ToString(), "SA");
        localeCountryDict.Add(TTSLocaleCountry.SENEGAL.ToString(), "SN");
        localeCountryDict.Add(TTSLocaleCountry.SERBIA.ToString(), "RS");
        localeCountryDict.Add(TTSLocaleCountry.SEYCHELLES.ToString(), "SC");
        localeCountryDict.Add(TTSLocaleCountry.SIERRA_LEONE.ToString(), "SL");
        localeCountryDict.Add(TTSLocaleCountry.SINGAPORE.ToString(), "SG");

        localeCountryDict.Add(TTSLocaleCountry.SINT_MAARTEN.ToString(), "SX");
        localeCountryDict.Add(TTSLocaleCountry.SLOVAKIA.ToString(), "SK");
        localeCountryDict.Add(TTSLocaleCountry.SLOVENIA.ToString(), "SI");
        localeCountryDict.Add(TTSLocaleCountry.SOLOMON_ISLANDS.ToString(), "SB");
        localeCountryDict.Add(TTSLocaleCountry.SOMALIA.ToString(), "SO");
        localeCountryDict.Add(TTSLocaleCountry.SOUTH_AFRICA.ToString(), "ZA");
        localeCountryDict.Add(TTSLocaleCountry.SOUTH_GEORGIA_AND_THE_SOUTH_SANDWICH_ISLANDS.ToString(), "GS");
        localeCountryDict.Add(TTSLocaleCountry.SOUTH_SUDAN.ToString(), "SS");
        localeCountryDict.Add(TTSLocaleCountry.SPAIN.ToString(), "ES");
        localeCountryDict.Add(TTSLocaleCountry.SRI_LANKA.ToString(), "LK");
        localeCountryDict.Add(TTSLocaleCountry.SUDAN.ToString(), "SD");
        localeCountryDict.Add(TTSLocaleCountry.SURINAME.ToString(), "SR");
        localeCountryDict.Add(TTSLocaleCountry.SVALBARD_AND_JAN_MAYEN.ToString(), "SJ");
        localeCountryDict.Add(TTSLocaleCountry.SWAZILAND.ToString(), "SZ");
        localeCountryDict.Add(TTSLocaleCountry.SWEDEN.ToString(), "SE");
        localeCountryDict.Add(TTSLocaleCountry.SWITZERLAND.ToString(), "CH");
        localeCountryDict.Add(TTSLocaleCountry.SYRIAN_ARAB_REPUBLIC.ToString(), "SY");

        localeCountryDict.Add(TTSLocaleCountry.TAIWAN.ToString(), "TW");
        localeCountryDict.Add(TTSLocaleCountry.TAJIKISTAN.ToString(), "TJ");
        localeCountryDict.Add(TTSLocaleCountry.TANZANIA.ToString(), "TZ");
        localeCountryDict.Add(TTSLocaleCountry.THAILAND.ToString(), "TH");
        localeCountryDict.Add(TTSLocaleCountry.TIMOR_LESTE.ToString(), "TL");
        localeCountryDict.Add(TTSLocaleCountry.TOGO.ToString(), "TG");
        localeCountryDict.Add(TTSLocaleCountry.TOKELAU.ToString(), "TK");
        localeCountryDict.Add(TTSLocaleCountry.TONGA.ToString(), "TO");
        localeCountryDict.Add(TTSLocaleCountry.TRINIDAD_AND_TOBAGO.ToString(), "TT");
        localeCountryDict.Add(TTSLocaleCountry.TUNISIA.ToString(), "TN");
        localeCountryDict.Add(TTSLocaleCountry.TURKEY.ToString(), "TR");
        localeCountryDict.Add(TTSLocaleCountry.TURKMENISTAN.ToString(), "TM");
        localeCountryDict.Add(TTSLocaleCountry.TURKS_AND_CAICOS_ISLANDS.ToString(), "TC");
        localeCountryDict.Add(TTSLocaleCountry.TUVALU.ToString(), "TV");
        localeCountryDict.Add(TTSLocaleCountry.UGANDA.ToString(), "UG");
        localeCountryDict.Add(TTSLocaleCountry.UKRAINE.ToString(), "UA");

        localeCountryDict.Add(TTSLocaleCountry.UNITED_ARAB_EMIRATES.ToString(), "AE");
        localeCountryDict.Add(TTSLocaleCountry.UNITED_KINGDOM.ToString(), "GB");
        localeCountryDict.Add(TTSLocaleCountry.UNITED_STATES.ToString(), "US");
        localeCountryDict.Add(TTSLocaleCountry.UNITED_STATES_MINOR_OUTLYING_ISLANDS.ToString(), "UM");
        localeCountryDict.Add(TTSLocaleCountry.URUGUAY.ToString(), "UY");
        localeCountryDict.Add(TTSLocaleCountry.UZBEKISTAN.ToString(), "UZ");
        localeCountryDict.Add(TTSLocaleCountry.VANUATU.ToString(), "VU");
        localeCountryDict.Add(TTSLocaleCountry.VATICAN_CITY.ToString(), "VA");
        localeCountryDict.Add(TTSLocaleCountry.VENEZUELA.ToString(), "VE");
        localeCountryDict.Add(TTSLocaleCountry.VIETNAM.ToString(), "VN");
        localeCountryDict.Add(TTSLocaleCountry.VIRGIN_ISLANDS_US.ToString(), "VI");
        localeCountryDict.Add(TTSLocaleCountry.WALLIS_AND_FUTUNA.ToString(), "WF");
        localeCountryDict.Add(TTSLocaleCountry.WESTERN_SAHARA.ToString(), "EH");
        localeCountryDict.Add(TTSLocaleCountry.YEMEN.ToString(), "YE");

        localeCountryDict.Add(TTSLocaleCountry.ZAMBIA.ToString(), "ZM");
        localeCountryDict.Add(TTSLocaleCountry.ZIMBABWE.ToString(), "ZW");
    }

    #endregion LoadLocaleCountry


    #region LoadCountryIS02

    private void LoadCountryISO2AlphaCountryName()
    {
        countryISO2AlphaDict.Add("AF", TTSLocaleCountry.AFGHANISTAN);
        countryISO2AlphaDict.Add("AX", TTSLocaleCountry.ALAND_ISLANDS);
        countryISO2AlphaDict.Add("AL", TTSLocaleCountry.ALBANIA);
        //countryISO2Alpha.Add("", CountryISO2Alpha.ALDERNEY);
        countryISO2AlphaDict.Add("DZ", TTSLocaleCountry.ALGERIA);
        countryISO2AlphaDict.Add("AS", TTSLocaleCountry.AMERICAN_SAMOA);
        countryISO2AlphaDict.Add("AD", TTSLocaleCountry.ANDORRA);
        countryISO2AlphaDict.Add("AO", TTSLocaleCountry.ANGOLA);
        countryISO2AlphaDict.Add("AI", TTSLocaleCountry.ANGUILLA);
        countryISO2AlphaDict.Add("AQ", TTSLocaleCountry.ANTARCTICA);
        countryISO2AlphaDict.Add("AG", TTSLocaleCountry.ANTIGUA_AND_BARBUDA);
        countryISO2AlphaDict.Add("AR", TTSLocaleCountry.ARGENTINA);
        countryISO2AlphaDict.Add("AM", TTSLocaleCountry.ARMENIA);
        countryISO2AlphaDict.Add("AW", TTSLocaleCountry.ARUBA);
        //countryISO2Alpha.Add("", CountryISO2Alpha.ASCENSION_ISLAND);
        countryISO2AlphaDict.Add("AU", TTSLocaleCountry.AUSTRALIA);
        countryISO2AlphaDict.Add("AT", TTSLocaleCountry.AUSTRIA);
        countryISO2AlphaDict.Add("AZ", TTSLocaleCountry.AZERBAIJAN);
		
        countryISO2AlphaDict.Add("BS", TTSLocaleCountry.BAHAMAS);
        countryISO2AlphaDict.Add("BH", TTSLocaleCountry.BAHRAIN);
        countryISO2AlphaDict.Add("BD", TTSLocaleCountry.BANGLADESH);
        countryISO2AlphaDict.Add("BB", TTSLocaleCountry.BARBADOS);
        countryISO2AlphaDict.Add("BY", TTSLocaleCountry.BELARUS);
        countryISO2AlphaDict.Add("BE", TTSLocaleCountry.BELGIUM);
        countryISO2AlphaDict.Add("BZ", TTSLocaleCountry.BELIZE);
        countryISO2AlphaDict.Add("BJ", TTSLocaleCountry.BENIN);
        countryISO2AlphaDict.Add("BM", TTSLocaleCountry.BERMUDA);
        countryISO2AlphaDict.Add("BT", TTSLocaleCountry.BHUTAN);
        countryISO2AlphaDict.Add("BO", TTSLocaleCountry.BOLIVIA);
        countryISO2AlphaDict.Add("BQ", TTSLocaleCountry.BONAIRE_ST_EUSTATIUS_AND_SABA);
        countryISO2AlphaDict.Add("BA", TTSLocaleCountry.BOSNIA_AND_HERZEGOVINA);
        countryISO2AlphaDict.Add("BW", TTSLocaleCountry.BOTSWANA);
        countryISO2AlphaDict.Add("BV", TTSLocaleCountry.BOUVET_ISLAND);
        countryISO2AlphaDict.Add("BR", TTSLocaleCountry.BRAZIL);
		
        countryISO2AlphaDict.Add("IO", TTSLocaleCountry.BRITISH_INDIAN_OCEAN_TERRITORY);
        countryISO2AlphaDict.Add("BN", TTSLocaleCountry.BRUNEI_DARUSSALAM);
        countryISO2AlphaDict.Add("BG", TTSLocaleCountry.BULGARIA);
        countryISO2AlphaDict.Add("BF", TTSLocaleCountry.BURKINA_FASO);
        countryISO2AlphaDict.Add("BI", TTSLocaleCountry.BURUNDI);
        countryISO2AlphaDict.Add("KH", TTSLocaleCountry.CAMBODIA);
        countryISO2AlphaDict.Add("CM", TTSLocaleCountry.CAMEROON);
        countryISO2AlphaDict.Add("CA", TTSLocaleCountry.CANADA);
        countryISO2AlphaDict.Add("CV", TTSLocaleCountry.CAPE_VERDE);
        countryISO2AlphaDict.Add("KY", TTSLocaleCountry.CAYMAN_ISLANDS);
        countryISO2AlphaDict.Add("CF", TTSLocaleCountry.CENTRAL_AFRICAN_REPUBLIC);
        countryISO2AlphaDict.Add("TD", TTSLocaleCountry.CHAD);
        //countryISO2Alpha.Add("", CountryISO2Alpha.CHANNEL_ISLANDS);
        countryISO2AlphaDict.Add("CL", TTSLocaleCountry.CHILE);
        countryISO2AlphaDict.Add("CN", TTSLocaleCountry.CHINA);
		
        countryISO2AlphaDict.Add("CX", TTSLocaleCountry.CHRISTMAS_ISLAND);
        countryISO2AlphaDict.Add("CC", TTSLocaleCountry.COCOS_ISLANDS);
        countryISO2AlphaDict.Add("CO", TTSLocaleCountry.COLOMBIA);
        countryISO2AlphaDict.Add("KM", TTSLocaleCountry.COMOROS);
        countryISO2AlphaDict.Add("CG", TTSLocaleCountry.CONGO);
        countryISO2AlphaDict.Add("CK", TTSLocaleCountry.COOK_ISLANDS);
        countryISO2AlphaDict.Add("CR", TTSLocaleCountry.COSTA_RICA);
        countryISO2AlphaDict.Add("CI", TTSLocaleCountry.COTE_DIVOIRE);
        countryISO2AlphaDict.Add("HR", TTSLocaleCountry.CROATIA);
        countryISO2AlphaDict.Add("CU", TTSLocaleCountry.CUBA);
        countryISO2AlphaDict.Add("CW", TTSLocaleCountry.CURACAO);
        countryISO2AlphaDict.Add("CY", TTSLocaleCountry.CYPRUS);
        countryISO2AlphaDict.Add("CZ", TTSLocaleCountry.CZECH_REPUBLIC);
        countryISO2AlphaDict.Add("DK", TTSLocaleCountry.DENMARK);
        countryISO2AlphaDict.Add("DJ", TTSLocaleCountry.DJIBOUTI);
		
        countryISO2AlphaDict.Add("DM", TTSLocaleCountry.DOMINICA);
        countryISO2AlphaDict.Add("DO", TTSLocaleCountry.DOMINICAN_REPUBLIC);
        countryISO2AlphaDict.Add("EC", TTSLocaleCountry.ECUADOR);
        countryISO2AlphaDict.Add("SV", TTSLocaleCountry.EL_SALVADOR);
        countryISO2AlphaDict.Add("GQ", TTSLocaleCountry.EQUATORIAL_GUINEA);
        countryISO2AlphaDict.Add("ER", TTSLocaleCountry.ERITREA);
        countryISO2AlphaDict.Add("EE", TTSLocaleCountry.ESTONIA);
        countryISO2AlphaDict.Add("ET", TTSLocaleCountry.ETHIOPIA);
        countryISO2AlphaDict.Add("FO", TTSLocaleCountry.FAEROE_ISLANDS);
        countryISO2AlphaDict.Add("FK", TTSLocaleCountry.FALKLAND_ISLANDS);
        countryISO2AlphaDict.Add("FJ", TTSLocaleCountry.FIJI);
        countryISO2AlphaDict.Add("FI", TTSLocaleCountry.FINLAND);
        countryISO2AlphaDict.Add("FR", TTSLocaleCountry.FRANCE);
		
        countryISO2AlphaDict.Add("GF", TTSLocaleCountry.FRENCH_GUIANA);
        countryISO2AlphaDict.Add("PF", TTSLocaleCountry.FRENCH_POLYNESIA);
        countryISO2AlphaDict.Add("TF", TTSLocaleCountry.FRENCH_SOUTHERN_TERRITORIES);
        countryISO2AlphaDict.Add("GA", TTSLocaleCountry.GABON);
        countryISO2AlphaDict.Add("GM", TTSLocaleCountry.GAMBIA_THE);
        countryISO2AlphaDict.Add("GH", TTSLocaleCountry.GHANA);
        countryISO2AlphaDict.Add("GI", TTSLocaleCountry.GIBRALTAR);
        //countryISO2Alpha.Add("GB", CountryISO2Alpha.GREAT_BRITAIN);
        countryISO2AlphaDict.Add("GR", TTSLocaleCountry.GREECE);
        countryISO2AlphaDict.Add("GL", TTSLocaleCountry.GREENLAND);
        countryISO2AlphaDict.Add("GD", TTSLocaleCountry.GRENADA);
        countryISO2AlphaDict.Add("GP", TTSLocaleCountry.GUADELOUPE);
        countryISO2AlphaDict.Add("GU", TTSLocaleCountry.GUAM);
        countryISO2AlphaDict.Add("GT", TTSLocaleCountry.GUATEMALA);
		
        countryISO2AlphaDict.Add("GG", TTSLocaleCountry.GUERNSEY);
        countryISO2AlphaDict.Add("GN", TTSLocaleCountry.GUINEA);
        countryISO2AlphaDict.Add("GW", TTSLocaleCountry.GUINEA_BISSAU);
        countryISO2AlphaDict.Add("GY", TTSLocaleCountry.GUYANA);
        countryISO2AlphaDict.Add("HT", TTSLocaleCountry.HAITI);
        countryISO2AlphaDict.Add("HM", TTSLocaleCountry.HEARD_ISLAND_AND_MCDONALD_ISLANDS);
        countryISO2AlphaDict.Add("HN", TTSLocaleCountry.HONDURAS);
        countryISO2AlphaDict.Add("HK", TTSLocaleCountry.HONGKONG);
        countryISO2AlphaDict.Add("HU", TTSLocaleCountry.HUNGARY);
        countryISO2AlphaDict.Add("IS", TTSLocaleCountry.ICELAND);
        countryISO2AlphaDict.Add("IN", TTSLocaleCountry.INDIA);
        countryISO2AlphaDict.Add("ID", TTSLocaleCountry.INDONESIA);
        //countryISO2Alpha.Add("", CountryISO2Alpha.INTERNATIONAL_ORGANIZATIONS);
        countryISO2AlphaDict.Add("IR", TTSLocaleCountry.IRAN);
        countryISO2AlphaDict.Add("IQ", TTSLocaleCountry.IRAQ);
        countryISO2AlphaDict.Add("IE", TTSLocaleCountry.IRELAND);
		
        countryISO2AlphaDict.Add("IM", TTSLocaleCountry.ISLE_OF_MAN);
        countryISO2AlphaDict.Add("IL", TTSLocaleCountry.ISRAEL);
        countryISO2AlphaDict.Add("IT", TTSLocaleCountry.ITALY);
        countryISO2AlphaDict.Add("JM", TTSLocaleCountry.JAMAICA);
        countryISO2AlphaDict.Add("JP", TTSLocaleCountry.JAPAN);
        countryISO2AlphaDict.Add("JE", TTSLocaleCountry.JERSEY);
        countryISO2AlphaDict.Add("JO", TTSLocaleCountry.JORDAN);
        countryISO2AlphaDict.Add("KZ", TTSLocaleCountry.KAZAKHSTAN);
        countryISO2AlphaDict.Add("KE", TTSLocaleCountry.KENYA);
        countryISO2AlphaDict.Add("KI", TTSLocaleCountry.KIRIBATI);
        countryISO2AlphaDict.Add("KP", TTSLocaleCountry.NORTH_KOREA);
        countryISO2AlphaDict.Add("KR", TTSLocaleCountry.SOUTH_KOREA);
        countryISO2AlphaDict.Add("KW", TTSLocaleCountry.KUWAIT);
        countryISO2AlphaDict.Add("KG", TTSLocaleCountry.KYRGYZSTAN);
        countryISO2AlphaDict.Add("LA", TTSLocaleCountry.LAO_PEOPLES_DEMOCRATIC_REPUBLIC);
        countryISO2AlphaDict.Add("LV", TTSLocaleCountry.LATVIA);
		
        countryISO2AlphaDict.Add("LB", TTSLocaleCountry.LEBANON);
        countryISO2AlphaDict.Add("LS", TTSLocaleCountry.LESOTHO);
        countryISO2AlphaDict.Add("LR", TTSLocaleCountry.LIBERIA);
        countryISO2AlphaDict.Add("LY", TTSLocaleCountry.LIBYA);
        countryISO2AlphaDict.Add("LI", TTSLocaleCountry.LIECHTENSTEIN);
        countryISO2AlphaDict.Add("LT", TTSLocaleCountry.LITHUANIA);
        countryISO2AlphaDict.Add("LU", TTSLocaleCountry.LUXEMBOURG);
        countryISO2AlphaDict.Add("MO", TTSLocaleCountry.MACAO);
        countryISO2AlphaDict.Add("MK", TTSLocaleCountry.MACEDONIA);
        countryISO2AlphaDict.Add("MG", TTSLocaleCountry.MADAGASCAR);
        countryISO2AlphaDict.Add("MW", TTSLocaleCountry.MALAWI);
        countryISO2AlphaDict.Add("MV", TTSLocaleCountry.MALDIVES);
        countryISO2AlphaDict.Add("ML", TTSLocaleCountry.MALI);
        countryISO2AlphaDict.Add("MT", TTSLocaleCountry.MALTA);
		
        countryISO2AlphaDict.Add("MH", TTSLocaleCountry.MARSHALL_ISLANDS);
        countryISO2AlphaDict.Add("MQ", TTSLocaleCountry.MARTINIQUE);
        countryISO2AlphaDict.Add("MR", TTSLocaleCountry.MAURITANIA);
        countryISO2AlphaDict.Add("MU", TTSLocaleCountry.MAURITIUS);
        countryISO2AlphaDict.Add("YT", TTSLocaleCountry.MAYOTTE);
        countryISO2AlphaDict.Add("MX", TTSLocaleCountry.MEXICO);
        countryISO2AlphaDict.Add("FM", TTSLocaleCountry.MICRONESIA);
        countryISO2AlphaDict.Add("MD", TTSLocaleCountry.MOLDOVA);
        countryISO2AlphaDict.Add("MC", TTSLocaleCountry.MONACO);
        countryISO2AlphaDict.Add("MN", TTSLocaleCountry.MONGOLIA);
        countryISO2AlphaDict.Add("ME", TTSLocaleCountry.MONTENEGRO);
        countryISO2AlphaDict.Add("MS", TTSLocaleCountry.MONTSERRAT);
        countryISO2AlphaDict.Add("MA", TTSLocaleCountry.MOROCCO);
        countryISO2AlphaDict.Add("MZ", TTSLocaleCountry.MOZAMBIQUE);
        countryISO2AlphaDict.Add("MM", TTSLocaleCountry.MYANMAR);
        countryISO2AlphaDict.Add("NA", TTSLocaleCountry.NAMIBIA);
		
        countryISO2AlphaDict.Add("NR", TTSLocaleCountry.NAURU);
        countryISO2AlphaDict.Add("NP", TTSLocaleCountry.NEPAL);
        countryISO2AlphaDict.Add("NL", TTSLocaleCountry.NETHERLANDS);
        countryISO2AlphaDict.Add("AN", TTSLocaleCountry.NETHERLANDS_ANTILLES);
        countryISO2AlphaDict.Add("NC", TTSLocaleCountry.NEW_CALEDONIA);
        countryISO2AlphaDict.Add("NZ", TTSLocaleCountry.NEW_ZEALAND);
        countryISO2AlphaDict.Add("NI", TTSLocaleCountry.NICARAGUA);
        countryISO2AlphaDict.Add("NE", TTSLocaleCountry.NIGER);
        countryISO2AlphaDict.Add("NU", TTSLocaleCountry.NIUE);
        countryISO2AlphaDict.Add("NF", TTSLocaleCountry.NORFOLK_ISLAND);
        countryISO2AlphaDict.Add("MP", TTSLocaleCountry.NORTHERN_MARIANA_ISLANDS);
        countryISO2AlphaDict.Add("NO", TTSLocaleCountry.NORWAY);
        countryISO2AlphaDict.Add("OM", TTSLocaleCountry.OMAN);
        countryISO2AlphaDict.Add("PK", TTSLocaleCountry.PAKISTAN);
		
        countryISO2AlphaDict.Add("PW", TTSLocaleCountry.PALAU);
        countryISO2AlphaDict.Add("PS", TTSLocaleCountry.PALESTINIAN_TERRITORIES);
        countryISO2AlphaDict.Add("PA", TTSLocaleCountry.PANAMA);
        countryISO2AlphaDict.Add("PG", TTSLocaleCountry.PAPUA_NEW_GUINEA);
        countryISO2AlphaDict.Add("PY", TTSLocaleCountry.PARAGUAY);
        countryISO2AlphaDict.Add("PE", TTSLocaleCountry.PERU);
        countryISO2AlphaDict.Add("PH", TTSLocaleCountry.PHILIPPINES);
        countryISO2AlphaDict.Add("PN", TTSLocaleCountry.PITCAIRN);
        countryISO2AlphaDict.Add("PL", TTSLocaleCountry.POLAND);
        countryISO2AlphaDict.Add("PT", TTSLocaleCountry.PORTUGAL);
        countryISO2AlphaDict.Add("PR", TTSLocaleCountry.PUERTO_RICO);
        countryISO2AlphaDict.Add("QA", TTSLocaleCountry.QATAR);
        countryISO2AlphaDict.Add("RE", TTSLocaleCountry.REUNION);
        countryISO2AlphaDict.Add("RO", TTSLocaleCountry.ROMANIA);
        countryISO2AlphaDict.Add("RU", TTSLocaleCountry.RUSSIAN_FEDERATION);
		
        countryISO2AlphaDict.Add("RW", TTSLocaleCountry.RWANDA);
        countryISO2AlphaDict.Add("BL", TTSLocaleCountry.SAINT_BARTHELEMY);
        countryISO2AlphaDict.Add("SH", TTSLocaleCountry.SAINT_HELENA);
        countryISO2AlphaDict.Add("KN", TTSLocaleCountry.SAINT_KITTS_AND_NEVIS);
        countryISO2AlphaDict.Add("LC", TTSLocaleCountry.SAINT_LUCIA);
        countryISO2AlphaDict.Add("MF", TTSLocaleCountry.SAINT_MARTIN);
        countryISO2AlphaDict.Add("PM", TTSLocaleCountry.SAINT_PIERRE_AND_MIQUELON);
        countryISO2AlphaDict.Add("VC", TTSLocaleCountry.SAINT_VINCENT_AND_THE_GRENADINES);
        countryISO2AlphaDict.Add("WS", TTSLocaleCountry.SAMOA);
        countryISO2AlphaDict.Add("SM", TTSLocaleCountry.SAN_MARINO);
        countryISO2AlphaDict.Add("ST", TTSLocaleCountry.SAO_TOME_AND_PRINCIPE);
        countryISO2AlphaDict.Add("SA", TTSLocaleCountry.SAUDI_ARABIA);
        countryISO2AlphaDict.Add("SN", TTSLocaleCountry.SENEGAL);
        countryISO2AlphaDict.Add("RS", TTSLocaleCountry.SERBIA);
        countryISO2AlphaDict.Add("SC", TTSLocaleCountry.SEYCHELLES);
        countryISO2AlphaDict.Add("SL", TTSLocaleCountry.SIERRA_LEONE);
        countryISO2AlphaDict.Add("SG", TTSLocaleCountry.SINGAPORE);
		
        countryISO2AlphaDict.Add("SX", TTSLocaleCountry.SINT_MAARTEN);
        countryISO2AlphaDict.Add("SK", TTSLocaleCountry.SLOVAKIA);
        countryISO2AlphaDict.Add("SI", TTSLocaleCountry.SLOVENIA);
        countryISO2AlphaDict.Add("SB", TTSLocaleCountry.SOLOMON_ISLANDS);
        countryISO2AlphaDict.Add("SO", TTSLocaleCountry.SOMALIA);
        countryISO2AlphaDict.Add("ZA", TTSLocaleCountry.SOUTH_AFRICA);
        countryISO2AlphaDict.Add("GS", TTSLocaleCountry.SOUTH_GEORGIA_AND_THE_SOUTH_SANDWICH_ISLANDS);
        countryISO2AlphaDict.Add("SS", TTSLocaleCountry.SOUTH_SUDAN);
        countryISO2AlphaDict.Add("ES", TTSLocaleCountry.SPAIN);
        countryISO2AlphaDict.Add("LK", TTSLocaleCountry.SRI_LANKA);
        countryISO2AlphaDict.Add("SD", TTSLocaleCountry.SUDAN);
        countryISO2AlphaDict.Add("SR", TTSLocaleCountry.SURINAME);
        countryISO2AlphaDict.Add("SJ", TTSLocaleCountry.SVALBARD_AND_JAN_MAYEN);
        countryISO2AlphaDict.Add("SZ", TTSLocaleCountry.SWAZILAND);
        countryISO2AlphaDict.Add("SE", TTSLocaleCountry.SWEDEN);
        countryISO2AlphaDict.Add("CH", TTSLocaleCountry.SWITZERLAND);
        countryISO2AlphaDict.Add("SY", TTSLocaleCountry.SYRIAN_ARAB_REPUBLIC);
		
        countryISO2AlphaDict.Add("TW", TTSLocaleCountry.TAIWAN);
        countryISO2AlphaDict.Add("TJ", TTSLocaleCountry.TAJIKISTAN);
        countryISO2AlphaDict.Add("", TTSLocaleCountry.TANGANYIKA);
        countryISO2AlphaDict.Add("TZ", TTSLocaleCountry.TANZANIA);
        countryISO2AlphaDict.Add("TH", TTSLocaleCountry.THAILAND);
        countryISO2AlphaDict.Add("TL", TTSLocaleCountry.TIMOR_LESTE);
        countryISO2AlphaDict.Add("TG", TTSLocaleCountry.TOGO);
        countryISO2AlphaDict.Add("TK", TTSLocaleCountry.TOKELAU);
        countryISO2AlphaDict.Add("TO", TTSLocaleCountry.TONGA);
        countryISO2AlphaDict.Add("TT", TTSLocaleCountry.TRINIDAD_AND_TOBAGO);
        countryISO2AlphaDict.Add("TN", TTSLocaleCountry.TUNISIA);
        countryISO2AlphaDict.Add("TR", TTSLocaleCountry.TURKEY);
        countryISO2AlphaDict.Add("TM", TTSLocaleCountry.TURKMENISTAN);
        countryISO2AlphaDict.Add("TC", TTSLocaleCountry.TURKS_AND_CAICOS_ISLANDS);
        countryISO2AlphaDict.Add("TV", TTSLocaleCountry.TUVALU);
        countryISO2AlphaDict.Add("UG", TTSLocaleCountry.UGANDA);
        countryISO2AlphaDict.Add("UA", TTSLocaleCountry.UKRAINE);
		
        countryISO2AlphaDict.Add("AE", TTSLocaleCountry.UNITED_ARAB_EMIRATES);
        countryISO2AlphaDict.Add("GB", TTSLocaleCountry.UNITED_KINGDOM);
        countryISO2AlphaDict.Add("US", TTSLocaleCountry.UNITED_STATES);
        countryISO2AlphaDict.Add("UM", TTSLocaleCountry.UNITED_STATES_MINOR_OUTLYING_ISLANDS);
        countryISO2AlphaDict.Add("UY", TTSLocaleCountry.URUGUAY);
        countryISO2AlphaDict.Add("UZ", TTSLocaleCountry.UZBEKISTAN);
        countryISO2AlphaDict.Add("VU", TTSLocaleCountry.VANUATU);
        countryISO2AlphaDict.Add("VA", TTSLocaleCountry.VATICAN_CITY);
        countryISO2AlphaDict.Add("VE", TTSLocaleCountry.VENEZUELA);
        countryISO2AlphaDict.Add("VN", TTSLocaleCountry.VIETNAM);
        countryISO2AlphaDict.Add("VI", TTSLocaleCountry.VIRGIN_ISLANDS_US);
        //countryISO2Alpha.Add("", CountryISO2Alpha.YUGOSLAVIA);
        countryISO2AlphaDict.Add("WF", TTSLocaleCountry.WALLIS_AND_FUTUNA);
        countryISO2AlphaDict.Add("EH", TTSLocaleCountry.WESTERN_SAHARA);
        countryISO2AlphaDict.Add("YE", TTSLocaleCountry.YEMEN);
		
        countryISO2AlphaDict.Add("ZM", TTSLocaleCountry.ZAMBIA);
        //countryISO2Alpha.Add("", CountryISO2Alpha.ZANZIBAR);
        countryISO2AlphaDict.Add("ZW", TTSLocaleCountry.ZIMBABWE);
    }

    #endregion LoadCountryIS02

    public string GetCountryISO2Alpha(TTSLocaleCountry country)
    {
        string countryISO2Alpha = localeCountryDict[country.ToString()];
        return countryISO2Alpha;
    }

    public TTSLocaleCountry GetCountry(string countryISO2Alpha)
    {
        TTSLocaleCountry country = (TTSLocaleCountry)countryISO2AlphaDict[countryISO2Alpha];
        return country;
    }

    public bool hasInitialized()
    {
        return hasInit;
    }

    public void Initialize()
    {
        if (!hasInit)
        {
            hasInit = true;
            #if UNITY_ANDROID
            if (Application.platform == RuntimePlatform.Android)
            {
                jo.CallStatic("init");
                SetTextToSpeechCallbackListener(OnTTSInit, OnTTSGetLocaleCountry, OnTTSSetLocale, OnTTSStartSpeech, OnTTSEndSpeech, OnTTSErrorSpeech);
            }
            else
            {
                AUP.Utils.Message(TAG, "warning: must run in actual android device");
            }
            #endif
        }
        else
        {
            AUP.Utils.Message(TAG, "TTS is already initialized calling this method is useless");
        }
    }

    public void RegisterBroadcastEvent()
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("registerBroadcastEvent");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    public void UnRegisterBroadcastEvent()
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("unregisterBroadcastEvent");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    private void SetTextToSpeechCallbackListener(
        Action <int>OnInit
		, Action <string>OnGetLocaleCountry
		, Action <int>OnSetLocale
		, Action <string>OnStartSpeech
		, Action <string>OnDoneSpeech
		, Action <string>OnErrorSpeech
    )
    {

        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            TTSCallback ttsCallback = new TTSCallback();
            ttsCallback.OnInit = OnInit;
            ttsCallback.OnGetLocaleCountry = OnGetLocaleCountry;
            ttsCallback.OnSetLocale = OnSetLocale;
            ttsCallback.OnStartSpeech = OnStartSpeech;
            ttsCallback.OnDoneSpeech = OnDoneSpeech;
            ttsCallback.OnErrorSpeech = OnErrorSpeech;
			
            Debug.Log("[SpeechPLugin] SetTextToSpeechCallbackListener  ttsCallback " + ttsCallback);
			
            jo.CallStatic("setTextToSpeechCallbackListener", ttsCallback);
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }


	
    public void CheckTTSData()
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("checkTTSData");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    private void GetAvailableLocale()
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("getAvailableLocale");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    public void SetLocaleByCountry(string localeCountry)
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("setLocaleByCountry", localeCountry);
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    public void SetLocale(SpeechLocale speechLocale)
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            int locale = (int)speechLocale;
            jo.CallStatic("setLocale", locale);
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    public void SetPitch(float pitch)
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("setPitch", pitch);
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    public void SetSpeechRate(float speechRate)
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("setSpeechRate", speechRate);
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    public void SpeakOut(string whatToSay, string utteranceId)
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("speakOut", whatToSay, utteranceId);
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }


    /// <summary>
    /// Stop Speech
    /// </summary>
    public void Stop()
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("stop");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    public void ShutDownTextToSpeechService()
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            jo.CallStatic("shutDownTextToSpeechService");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
    }

    /// <summary>
    /// Checks whether the TTS engine is busy speaking
    /// </summary>
    /// <returns><c>true</c> if TTS engine is busy speaking; otherwise, <c>false</c>.</returns>
    public bool IsSpeaking()
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            return jo.CallStatic<bool>("isSpeaking");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
		
        return false;
    }


    /// <summary>
    /// Checks the TTS data activity. if this activity is not avaible tts will not work
    /// </summary>
    /// <returns><c>true</c>, if TTS data activity was checked, <c>false</c> otherwise.</returns>
    public bool CheckTTSDataActivity()
    {
        #if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            return jo.CallStatic<bool>("checkTTSDataActivity");
        }
        else
        {
            AUP.Utils.Message(TAG, "warning: must run in actual android device");
        }
        #endif
		
        return false;
    }

    public bool isInitialized()
    {
        return hasTTSInit;
    }

    public bool CheckLocale(TTSLocaleCountry ttsCountry)
    {
        bool found = false;
        if (localeCountryISO2AlphaSet != null)
        {
            string countryISO2Alpha = GetCountryISO2Alpha(ttsCountry);
			
            foreach (string country in localeCountryISO2AlphaSet)
            {
                Debug.Log("CheckLocale country: " + country + " target: " + countryISO2Alpha);
                if (country.Equals(countryISO2Alpha, StringComparison.Ordinal))
                {
                    found = true;
                    break;
                }
            }
        }
		
        return found;
    }

    private void OnTTSInit(int status)
    {
        Debug.Log("[TextToSpeechDemo] OnInit status: " + status);
        if (status == 1)
        {
            GetAvailableLocale();
        }

        if (null != Init)
        {
            Init(status);
        }

        hasTTSInit = true;
    }

    private void OnTTSGetLocaleCountry(string localeCountry)
    {
        Debug.Log("[TextToSpeechDemo] OnGetLocaleCountry localeCountry: " + localeCountry);
        localeCountryISO2AlphaSet = localeCountry.Split(',');

        if (null != GetLocaleCountry)
        {
            GetLocaleCountry(localeCountry);
        }
    }

    private void OnTTSSetLocale(int status)
    {
        Debug.Log("[TextToSpeechDemo] OnSetLocale status: " + status);

        if (null != ChangeLocale)
        {
            ChangeLocale(status);
        }
    }

    private void OnTTSStartSpeech(string utteranceId)
    {
        Debug.Log("[TextToSpeechDemo] OnStartSpeech utteranceId: " + utteranceId);
        if (null != StartSpeech)
        {
            StartSpeech(utteranceId);
        }
    }

    private void OnTTSEndSpeech(string utteranceId)
    {
        Debug.Log("[TextToSpeechDemo] OnEndSpeech utteranceId: " + utteranceId);
        if (null != EndSpeech)
        {
            EndSpeech(utteranceId);
        }
    }

    private void OnTTSErrorSpeech(string utteranceId)
    {		
        Debug.Log("[TextToSpeechDemo] OnErrorSpeech utteranceId: " + utteranceId);

        if (null != ErrorSpeech)
        {
            ErrorSpeech(utteranceId);
        }
    }
}