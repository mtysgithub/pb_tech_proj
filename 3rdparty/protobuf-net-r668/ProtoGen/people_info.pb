syntax = "proto2";
message PeopleInfo{
  required string name = 1;
  optional int32 age = 2;
  optional  int32 id_card_no = 3;
  repeated string friend_name = 4;
}
