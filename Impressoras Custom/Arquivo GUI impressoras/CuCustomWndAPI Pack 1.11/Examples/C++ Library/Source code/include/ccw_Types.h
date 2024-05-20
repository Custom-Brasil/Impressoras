// File Revision
// Rev 1.02
//
// - rel 1.00: first release
// - rel 1.01: Add CashDrawerType
// - rel 1.02: Add codepage into PrintFontStruct
//             Add TrPrintMode

// NOTE: Please DO NOT EDIT THIS FILE

///
/// \file ccw_Types.h
/// \brief Data and parameters definition.
///


#ifndef _CCW_TYPES_H__
#define _CCW_TYPES_H__

#include "ccw_Error.h"

#pragma pack(1)

// Common define

/// Lenght, in characters, of a standard string
#define MAXCHARBUFFER					1024

/// Service type definition
typedef char            int8;
/// Service type definition
typedef short           int16;
/// Service type definition
typedef int             int32;
/// Service type definition
typedef unsigned char   uint8;
/// Service type definition
typedef unsigned short  uint16;
/// Service type definition
typedef unsigned int    uint32;



/**********************************************************/
// Enum
/**********************************************************/

/// <summary>
/// Log verbosity. Set library log verbosity using \ref InitLibrary
/// </summary>
typedef enum : uint8
{
	/**
	* Log completely disabled
	*/
	CCW_LOG_DISABLED = 0,

	/**
	* Only log fatal errors
	*/
	CCW_LOG_FATAL = 1,

	/**
	* (normal) Log errors. Program will continue
	*/
	CCW_LOG_ERROR = 2,

	/**
	* Also log warning - something suspicious occured
	*/
	CCW_LOG_WARNING = 3,

	/**
	* Also log hints and informational messages
	*/
	CCW_LOG_INFO = 4,

	/**
	* Log functions methods messages
	*/
	CCW_LOG_FUNCTIONS = 5,

	/**
	* Log all messages, also debug messages. Use only if required
	*/
	CCW_LOG_DEBUG = 6,

	/**
	* Log all messages, also deep debug messages. Use only if required. Could slow down the communication
	*/
	CCW_LOG_DEEP_DEBUG = 7,

} CcwLogVerbosity;

/// <summary>
/// RAW Bit depth type
/// </summary>
typedef enum : BYTE
{
	/**
	* The raw image is Monochromatic
	*/
	MONOCHROMATIC_1BIT = 0,

	/**
	* The raw image is 256 level greyscale
	*/
	GREYSCALE_8BIT = 1,

	/**
	* The raw image is 24 bit RGB
	*/
	RGB_24BIT = 2,

} RAWBitDepth;

/// <summary>
/// Usb communication interface type
/// </summary>
typedef enum : BYTE
{
	/**
	* The usb interface is a PRINTER
	*/
	USB_INT_PRINTER = 0,

} UsbInterfaceType;

/// <summary>
/// Communication port
/// </summary>
typedef enum : BYTE
{
	/**
	* Usb
	*/
	COMMTYPE_USB = 1,

	/**
	* Serial / Com
	*/
	COMMTYPE_COM = 2,

	/**
	* Net / Ethernet / Wifi
	*/
	COMMTYPE_NET = 3,

	/**
	* Unknown
	*/
	COMMTYPE_OTHER = 0xFF,

} CommunicationPortType;

/// <summary>
/// Print Text : Font scale
/// </summary>
typedef enum : BYTE
{
	/**
	* Indicates that the Font Size is 1X
	*/
	FONT_SIZE_X1 = 0,
	/**
	* Indicates that the Font Size is 2X
	*/
	FONT_SIZE_X2 = 1,
	/**
	* Indicates that the Font Size is 3X
	*/
	FONT_SIZE_X3 = 2,
	/**
	* Indicates that the Font Size is 4X
	*/
	FONT_SIZE_X4 = 3,
	/**
	* Indicates that the Font Size is 5X
	*/
	FONT_SIZE_X5 = 4,
	/**
	* Indicates that the Font Size is 6X
	*/
	FONT_SIZE_X6 = 5,
	/**
	* Indicates that the Font Size is 7X
	*/
	FONT_SIZE_X7 = 6,
	/**
	* Indicates that the Font Size is 8X
	*/
	FONT_SIZE_X8 = 7,

} FontSize;

/// <summary>
/// Print Text : text justification
/// </summary>
typedef enum : BYTE
{
	/**
	* Indicates that the Font Justification is Left
	*/
	FONT_JUSTIFICATION_LEFT = 0,
	/**
	* Indicates that the Font Justification is Center
	*/
	FONT_JUSTIFICATION_CENTER = 1,
	/**
	* Indicates that the Font Justification is Right
	*/
	FONT_JUSTIFICATION_RIGHT = 2,

} FontJustification;

/// <summary>
/// Print Text : Font type
/// @note Refer to the User Manual to verify the font supported by each device
/// </summary>
typedef enum : BYTE
{
	/**
	* Indicates that the Font 1
	*/
	FONT_TYPE_1 = 0,
	/**
	* Indicates that the Font 2
	*/
	FONT_TYPE_2 = 1,
	/**
	* Indicates that the Font 3
	*/
	FONT_TYPE_3 = 2,
	/**
	* Indicates that the Font 4
	*/
	FONT_TYPE_4 = 3,

} FontType;

/// <summary>
/// Print Text : Font international Charset
/// </summary>
typedef enum : BYTE
{
	/**
	* Indicates that the International Charset is Default
	*/
	FONT_CS_DEFAULT = 0,

} FontInternationalCharset;

/// <summary>
/// Paper cut type
/// </summary>
typedef enum : BYTE
{

	/**
	* Indicates to perform a Total Cut
	*/
	CUT_TOTAL = 0,
	/**
	* Indicates to perform a Partial Cut
	*/
	CUT_PARTIAL = 1

} CutType;

/// <summary>
/// Eject paper type
/// </summary>
typedef enum : BYTE
{

	/**
	* Indicates to perform a Eject
	*/
	EJ_EJECT = 0,
	/**
	* Indicates to perform a Retract
	*/
	EJ_RETRACT = 1,

} EjectType;

/// <summary>
/// Paper alignment type
/// </summary>
typedef enum : BYTE
{
	/**
	* Indicates to perform a Paper alignment under the printing line
	*/
	PALIGN_PRINT = 0,
	/**
	* Indicates to perform a Paper alignment under the cutting line
	*/
	PALIGN_CUT = 1,

} PaperAlignType;


/// <summary>
/// Cash Drawer connector pin
/// </summary>
typedef enum : BYTE
{
	/**
	* Indicates that the cash drawer selected is DRW1
	*/
	CASH_DRAWER_1 = 0,
	/**
	* Indicates that the cash drawer selected is DRW2
	*/
	CASH_DRAWER_2 = 1,

} CashDrawerType;

/// <summary>
/// Print Image : scaling type
/// </summary>
typedef enum : BYTE
{
	/**
	* Indicates that the Picture will be No scaling. Excess size will be ignored
	*/
	IMAGE_SCALE_NONE = 0,
	/**
	* Indicates that the Picture will be Scale to fit the paper width
	*/
	IMAGE_SCALE_TO_FIT = 1,
	/**
	* Indicates that the Picture will be Scale the image to width given by width parameter
	*/
	IMAGE_SCALE_TO_WIDTH = 2,

} ImageScale;

/// <summary>
/// Print Image : Image justification
/// </summary>
typedef enum : BYTE
{
	/**
	* Indicates that the Picture Justification is Left
	*/
	IMAGE_ALIGN_TO_LEFT = 0,
	/**
	* Indicates that the Picture Justification is Center
	*/
	IMAGE_ALIGN_TO_CENTER = 1,
	/**
	* Indicates that the Picture Justification is Right
	*/
	IMAGE_ALIGN_TO_RIGHT = 2,

} ImageAlign;

/// <summary>
/// Print Image : threshold mode to print the Image
/// </summary>
typedef enum : BYTE
{
	/**
	* Indicates that the Image will be converted to Black / White using Dithering way.
	* Best Quality for Photos and Graphics but slow(Default)
	*/
	IMAGE_GRMODE_DITHERING = 0,
	/**
	* Indicates that the Image will be converted to Black / White using Threshold way
	* Best Quality for Text, but not for graphics, but quick
	*/
	IMAGE_GRMODE_THRESHOLD = 1,

} ImageGRMode;

/// <summary>
/// Print Barcode : Barcode HRI Position
/// </summary>
typedef enum : BYTE
{
	/**
	* Indicates no HRI on the Barcode
	*/
	BARCODE_HRI_NONE = 0,
	/**
	* Indicates HRI on TOP of the Barcode
	*/
	BARCODE_HRI_TOP = 1,
	/**
	* Indicates HRI on BOTTOM of the Barcode
	*/
	BARCODE_HRI_BOTTOM = 2,
	/**
	* Indicates HRI on TOP and BOTTOM of the Barcode
	*/
	BARCODE_HRI_TOPBOTTOM = 3,

} BarcodeHRIPosition;

/// <summary>
/// Print Barcode : Barcode justification
/// </summary>
typedef enum : BYTE
{
	/**
	* Indicates that the Barcode Justification is Left
	*/
	BARCODE_ALIGN_TO_LEFT = 0,
	/**
	* Indicates that the Barcode Justification is Center
	*/
	BARCODE_ALIGN_TO_CENTER = 1,
	/**
	* Indicates that the Barcode Justification is Right
	*/
	BARCODE_ALIGN_TO_RIGHT = 2,

} BarcodeAlign;

/// <summary>
/// Print Barcode : Barcode type
/// </summary>
typedef enum : WORD
{
	/**
	* The barcode type isn't selected
	*/
	BARCODE_TYPE_NONE = 0,

	//************************************************************************************************
	// STATIC CONST : BARCODE 1D TYPE
	//************************************************************************************************

	/**
	* Indicates to print a CODEBAR Barcode
	*/
	BARCODE_TYPE_CODABAR = 1,
	/**
	* Indicates to print a UPCA Barcode
	*/
	BARCODE_TYPE_UPCA = 2,
	/**
	* Indicates to print a UPCE Barcode
	*/
	BARCODE_TYPE_UPCE = 3,
	/**
	* Indicates to print a EAN13 Barcode
	*/
	BARCODE_TYPE_EAN13 = 4,
	/**
	* Indicates to print a EAN8 Barcode
	*/
	BARCODE_TYPE_EAN8 = 5,
	/**
	* Indicates to print a CODE39 Barcode
	*/
	BARCODE_TYPE_CODE39 = 6,
	/**
	* Indicates to print a ITF Barcode
	*/
	BARCODE_TYPE_ITF = 7,
	/**
	* Indicates to print a CODE93 Barcode
	*/
	BARCODE_TYPE_CODE93 = 8,
	/**
	* Indicates to print a CODE128 Barcode
	*/
	BARCODE_TYPE_CODE128 = 9,
	/**
	* Indicates to print a CODE32 Barcode
	*/
	BARCODE_TYPE_CODE32 = 10,
	/**
	* Indicates to print a EAN128 Barcode
	*/
	BARCODE_TYPE_EAN128 = 11,

	//************************************************************************************************
	// STATIC CONST : BARCODE 2D TYPE
	//************************************************************************************************

	/**
	* Indicates to print a QRCODE Barcode 2D
	*/
	BARCODE_TYPE_QRCODE = 1001,

	/**
	* Indicates to print a PDF417 Barcode 2D
	*/
	BARCODE_TYPE_PDF417 = 1002,

	/**
	* Indicates to print a DATAMATRIX Barcode 2D
	*/
	BARCODE_TYPE_DATAMATRIX = 1003,

	/**
	* Indicates to print a AZTEC Barcode 2D
	*/
	BARCODE_TYPE_AZTEC = 1004,

} BarcodeType;

/// <summary>
/// Com port Settings : Baudrate
/// </summary>
typedef enum : DWORD
{
	/**
	* Indicates that the RS232 is set to auto baudrate
	*/
	COM_SETT_BAUDRATE_AUTO = 0,

	/**
	* Indicates that the RS232 baudrate to 1200
	*/
	COM_SETT_BAUDRATE_1200 = 1200,

	/**
	* Indicates that the RS232 baudrate to 2400
	*/
	COM_SETT_BAUDRATE_2400 = 2400,

	/**
	* Indicates that the RS232 baudrate to 4800
	*/
	COM_SETT_BAUDRATE_4800 = 4800,

	/**
	* Indicates that the RS232 baudrate to 9600
	*/
	COM_SETT_BAUDRATE_9600 = 9600,
	
	/**
	* Indicates that the RS232 baudrate to 19200
	*/
	COM_SETT_BAUDRATE_19200 = 19200,

	/**
	* Indicates that the RS232 baudrate to 38400
	*/
	COM_SETT_BAUDRATE_38400 = 38400,

	/**
	* Indicates that the RS232 baudrate to 57600
	*/
	COM_SETT_BAUDRATE_57600 = 57600,

	/**
	* Indicates that the RS232 baudrate to 115200
	*/
	COM_SETT_BAUDRATE_115200 = 115200,

} Com_Settings_Baudrate;

/// <summary>
/// Com port Settings : Parity
/// </summary>
typedef enum : BYTE
{
	/**
	* Indicates that the RS232 Parity None
	*/
	COM_SETT_PARITY_NONE = NOPARITY,

	/**
	* Indicates that the RS232 Parity Odd
	*/
	COM_SETT_PARITY_ODD = ODDPARITY,

	/**
	* Indicates that the RS232 Parity Even
	*/
	COM_SETT_PARITY_EVEN = EVENPARITY,

} Com_Settings_Parity;

/// <summary>
/// Com port Settings : Num Bits
/// </summary>
typedef enum : BYTE
{
	/**
	* Indicates that the RS232 Num bits 8
	*/
	COM_SETT_NBITS_8 = 8,

	/**
	* Indicates that the RS232 Num bits 7
	*/
	COM_SETT_NBITS_7 = 7,

} Com_Settings_NBits;

/// <summary>
/// Com port Settings : Stop Bits
/// </summary>
typedef enum : BYTE
{
	/**
	* Indicates that the RS232 Stop bits 1
	*/
	COM_SETT_STOPBITS_1 = ONESTOPBIT,

	/**
	* Indicates that the RS232 Stop bits 2
	*/
	COM_SETT_STOPBITS_2 = TWOSTOPBITS,

} Com_Settings_StopBits;

/// <summary>
/// Com port Settings : Flow Control
/// </summary>
typedef enum : BYTE
{
	/**
	* Indicates that the RS232 Flow Control None
	*/
	COM_SETT_FLOW_CONTROL_NONE = 0,

	/**
	* Indicates that the RS232 Flow Control Hardware
	*/
	COM_SETT_FLOW_CONTROL_HW = 1,

	/**
	* Indicates that the RS232 Flow Control Xon Xoff (Software)
	*/
	COM_SETT_FLOW_CONTROL_XONXOFF = 2,

} Com_Settings_FlowControl;

/// <summary>
/// Transaction Print
/// </summary>
typedef enum : BYTE
{
	/**
	* Indicates the library will buffer the print data until the function is called with the TP_TRANSACTION_STOP or TP_TRANSACTION_PRINT
	*/
	TP_TRANSACTION_REC = 0,

	/**
	* Indicates the library stop to store the print data
	*/
	TP_TRANSACTION_STOP = 1,

	/**
	* Indicates the library stop to store and prints all the print data stored
	*/
	TP_TRANSACTION_PRINT = 2,

	/**
	* Indicates the library cleans all the print data stored
	*/
	TP_TRANSACTION_CLEAN = 3,
	
} TrPrintMode;

/****************************** Structures ******************/

/**
* Printer Struct
*/
typedef struct {

	/** Printer Name
	*/
	wchar_t cPrinterName[MAXCHARBUFFER];

	/** TRUE if this printer is Online
	*/
	BOOL bPrinterOnline;

	/** TRUE if this is the default Printer
	*/
	BOOL bDefaultPrinter;

	/// Reserved fot future development. Do not use
	BYTE reserved[10240];
} PrinterStruct;

/**
* USB Device Struct
*/
typedef struct {


	/** Vid / Vendor ID
	*/
	WORD wVID;

	/** Pid / Product ID
	*/
	WORD wPID;	

	/** Serial Number
	*/
	wchar_t cSerialNumber[MAXCHARBUFFER];

	/** Printer Name (If available)
	*/
	wchar_t cPrinterName[MAXCHARBUFFER];

	/** Usb interface type
	*/
	UsbInterfaceType uIntType;

	/// Reserved fot future development. Do not use
	BYTE reserved[2043];
} USBDeviceStruct;

/**
* ETH/WIFI Device Struct
*/
typedef struct {

	/** IP Address
	*/
	wchar_t cIPAddress[MAXCHARBUFFER];

	/** IP Port
	*/
	DWORD dwIPPort;

	/** MAC Address
	*/
	wchar_t cMACAddress[MAXCHARBUFFER];

	/** Device Name
	*/
	wchar_t cDeviceName[MAXCHARBUFFER];

	/// Reserved fot future development. Do not use
	BYTE reserved[2048];
} ETHDeviceStruct;

/**
* COM Port Struct
*/
typedef struct {

	/** COM Port
	*/
	wchar_t cCOMPort[MAXCHARBUFFER];	

	/** Com port baudrate
	*/
	Com_Settings_Baudrate cBaudrate;

	/** Com port parity
	*/
	Com_Settings_Parity cParity;

	/** Com port bits
	*/
	Com_Settings_NBits cNBits;

	/** Com port number of stop bits
	*/
	Com_Settings_StopBits cStopBits;

	/** Com port flow control
	*/
	Com_Settings_FlowControl cFlowControl;

	/// Reserved fot future development. Do not use
	BYTE reserved[2048];
} COMPortStruct;

/**
* Print Font Struct
*/
typedef struct {

	/** Char width scale
	*/
	FontSize charWidth;	

	/** Char height scale
	*/
	FontSize charHeight;

	/** Print justification
	*/
	FontJustification justification;

	/** Font Type
	*/
	FontType charFontType;								

	/** International Font type
	*/
	FontInternationalCharset interCharSet;

	/** Font line spacing
	*/
	BYTE bLineSpacing;

	/** TRUE for italic font
	*/
	BOOL italic;

	/** TRUE for emphasized / bold font
	*/
	BOOL emphasized;

	/** TRUE for underline font
	*/
	BOOL underline;

	/** Print width (dots)
	*/
	WORD printWidth;

	/** Left margin (dots)
	*/
	WORD leftMarginValue;

	/** Specified codepage (set to 0 for the automatic selection)
	*/
	DWORD codepage;

	/// Reserved fot future development. Do not use
	BYTE reserved[1020];

} PrintFontStruct;
/**
* Init value to set the \ref PrintFontStruct to default
*/
#define PRINTFONTSTRUCT_INIT { FONT_SIZE_X1, FONT_SIZE_X1, FONT_JUSTIFICATION_LEFT, FONT_TYPE_1, FONT_CS_DEFAULT, 64, FALSE,FALSE,FALSE , 0, 0, 0, {0}}

/**
* Print Image Struct
*/
typedef struct {

	/** Image justification
	*/
	ImageAlign imageAlignMode;

	/** Left margin (dots). If this value is different to 0, the parameter imageAlignMode will be ignored
	*/
	DWORD leftMarginValue;	

	/** Width Value (dots)
	*/
	DWORD printScaleWidthValue;

	/** Image Scale
	*/
	ImageScale printScaleMode;

	/// Reserved fot future development. Do not use
	BYTE reserved[1023];
} PrintImageStruct;

/**
* Init value to set the \ref PrintImageStruct to default
*/
#define PRINTIMAGESTRUCT_INIT { IMAGE_ALIGN_TO_LEFT, 0, 0 , IMAGE_SCALE_NONE,  {0}}

/**
* Print Barcode Struct
*/
typedef struct {
	

	/** Left margin (dots), If this value is different to 0, the parameter brcAlignMode will be ignored
	*/
	DWORD leftMarginValue;

	/** Barcode Width (dots)
	If this value is < 10, the value is the width to the narrowest bar. Otherwise is the max barcode width in dots.
	*/
	DWORD dwBarcodeWidth;

	/** Barcode Height (dots)
	*/
	DWORD dwBarcodeHeight;

	/** Barcode justification
	*/
	BarcodeAlign brcAlignMode;

	/** Barcode HRI Position
	*/
	BarcodeHRIPosition brcHRIPosition;	

	/** Barcode type
	*/
	BarcodeType brcType;

	/// Reserved fot future development. Do not use
	BYTE reserved[1023];
} PrintBarcodeStruct;

/**
* Init value to set the \ref PrintBarcodeStruct to default
*/
#define PRINTBARCODESTRUCT_INIT {0, 0,0, BARCODE_ALIGN_TO_LEFT,BARCODE_HRI_BOTTOM, BARCODE_TYPE_NONE, {0}}


#pragma pack()

#endif



