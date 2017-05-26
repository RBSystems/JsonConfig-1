# JsonConfig
Crestron S#
This project is intended to create a series of SIMPL/S+ modules that will load and process a JSON config file from  user-designated location.
This is for use with a Crestron Control system, and won't work outside of the Crestron environment, due to sandboxing by Crestron.
Different file formats can be supported by creating a custom implementation of ISettingsReader and ISettingsWriter
and using that in the ConfigurationReader/ConfigurationWriter classes instead of the provided JsonSettingsReader/Writer.
Future enhancements include a mechanism to load the JSON classes for deserialization from a user provided dll to allow for custom JSON formats.
