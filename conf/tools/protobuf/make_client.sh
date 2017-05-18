PROTO_CONFIG_DIR="Y:/data/game/pbdfn/client"
CSHARP_OUTPUT="X:/Assets/ClientCode/ModelLayer/M_GameData/PBClientDefine.cs"
PROTO_CONFIG="client_config.proto"

PROTO_CONFIG_PLUGIN_DIR="Y:/data/game/pbdfn/client/plugins"
CSHARP_PLUGIN_OUTPUT="X:/Assets/Plugins/GameKit/Data/PBClientDefine.cs"
PROTO_CONFIG_PLUGIN="resource_build.proto"

cd ${PROTO_CONFIG_DIR}
Y:/tools/protobuf/protogen/protogen.exe -i:${PROTO_CONFIG} -o:${CSHARP_OUTPUT} -ns:NS_GAME.DATA -p:xml

cd ${PROTO_CONFIG_PLUGIN_DIR}
Y:/tools/protobuf/protogen/protogen.exe -i:${PROTO_CONFIG_PLUGIN} -o:${CSHARP_PLUGIN_OUTPUT} -ns:NS_GAME.DATA -p:xml

echo "Generate CS Files"
echo Press Enter to continue!
read n