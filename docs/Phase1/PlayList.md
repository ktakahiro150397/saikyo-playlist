# プレイリスト機能
プレイリストの管理を行う。

## プレイリスト属性
- プレイリスト名
- 対象URL
- 再生回数

## プレイリスト追加
- 手動でURLを入力し、追加
- Youtubeプレイリストからのインポート

## プレイリスト再生
- プレイリストを再生し、通常再生・ランダム再生・リピート再生
- 再生完了時、再生回数を記録

## プレイリスト共有
- プレイリストURLを共有して他ユーザーからも再生可能とする

<br>

# タグ機能
動画にタグ情報を付与する。

## タグ検索
- タグを参照してリストを作成する。

<br>

# 評価機能
動画に評価情報を付与する。

<br>

# プレイリスト実装

## データ構造

以下の変数を画面側で保持する。

|項目|項目名|内容|
|--|--|--|
|プレイリストID|`playListId`|プレイリストごとに採番されるユニークなID。|
|プレイリストデータ|`playList`|画面で再生するプレイリストのデータ。<br>データ構造は後述の通り。|
|再生中の連番|`currentIndex`|0から始まる現在再生中のプレイリストの連番|
|プレイリストの最大インデックス|`maxIndex`|プレイリストの最大の連番|

<br>

以下のデータの配列をサーバーサイドから取得する。

|項目|項目名|内容|
|--|--|--|
|サイト種類|`type`|以下のうちのいずれか。<br>`youtube` , `spotify` , `applemusic`|
|アイテムID|`itemId`|アイテムを特定するためのID。<br>Youtube : 動画ID|
|アイテム連番|`itemSeq`|0から始まるプレイリストに振られている整数。|
|タイトル|`title`|アイテムの元々の名称。<br/>Youtube : 動画名|
|タイトルエイリアス|`titleAlias`|ユーザーによって付けられたプレイリストへの別名。|
|再生回数|`playCount`|このプレイリスト内でアイテムが再生された回数。<br>(最後まで再生された場合に+1する)|

<br>

## 再生画面処理順序

1. 画面ロード時、サーバーサイドからプレイリスト情報を取得する。
1. 取得後、プレイリストの先頭を再生する。
1. 再生終了時、`playCount`を+1し、次のアイテムの再生を開始する。

## 作成する関数

- サーバーサイド
  - ロード時、プレイリストデータをJSON構造で返す
  - 指定したプレイリスト内のアイテムの再生回数を+1する

- クライアントサイド
  - 指定インデックスの曲を再生する : 初回ロード時0、リンククリック時そのインデックスを指定する

<br>

## DB構造

- PlayListHeaders

プレイリストのヘッダー情報を格納する。

|列名|型|内容|制約|
|--|--|--|--|
|Id|nvarchar(512) not null|プレイリストごとのユニークなID。<br>登録時、ULIDを採番する。|PK|
|AspNetUsersId|nvarchar(450) not null|このプレイリストを作成したユーザーのID。|PK / FK(AspNetUsers.Id)|
|Name|nvarchar(128) not null|プレイリストの名称。||
|TimeStamp|varbinary(max) not null|楽観的同時実行制御のためのタイムスタンプ。||

<br>

- PlayListDetails

プレイリストの詳細情報を格納する。

|列名|型|内容|制約|
|--|--|--|--|
|PlayListHeadersId|nvarchar(512) not null|`PlayListDetails`テーブルで採番したID。|PK / FK|
|itemSeq|int not null|0から始まるプレイリストの連番。|PK|
|type|nvarchar(32) not null|プレイリストのプラットフォーム。<br>`youtube`,`spotify`,`applemusic`のいずれか。|PK|
|itemId|nvarchar(512) not null|アイテムを特定するためのプラットフォームごとのID。|PK|
|title|nvarchar(500) not null|アイテムの元々の名称。<br>Youtube:動画名||
|titleAlias|nvarchar(500) null|ユーザーによって付けられたアイテムの別名。||
|playCount|int null|アイテムがプレイリスト内で最後まで再生された数。||
|TimeStamp|varbinary(max) not null|楽観的同時実行制御のためのタイムスタンプ。||

