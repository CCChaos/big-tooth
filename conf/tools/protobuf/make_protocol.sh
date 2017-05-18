PROTO_CONFIG_DIR="Y:/data/game/pbdfn"
PROTO_CONFIG="
test.hxx
client_config.hxx
resource_config.hxx
"

DESC_OUTPUT="Y:/data/game/pbdfn/config_desc.bin"
CSHARP_OUTPUT=X:/Assets/ClientLogic/ModelLayer/M_GameData/DataDefine.cs
CPLUSPLUS_OUTPUT=./

cd ${PROTO_CONFIG_DIR}

Y:/tools/protobuf/protoc/protoc.exe -I=. --descriptor_set_out=${DESC_OUTPUT} ${PROTO_CONFIG}
echo "Generate CPP Files"
Y:/tools/protobuf/protogen/protogen.exe -i:${DESC_OUTPUT} -o:${CSHARP_OUTPUT} -ns:NS_GAMEDATA -p:xml
echo "Generate CS Files"
echo Press Enter to continue!
read n