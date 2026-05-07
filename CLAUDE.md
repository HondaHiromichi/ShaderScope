# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## プロジェクト概要

ShaderScope は **Shader Viewer アプリ**。ユーザーがシェーダーの種類を選択すると、それが適用されたオブジェクトを 3D ビュー上で確認できる。Unity **2022.3.62f3** (LTS) / **URP 14.0.12** ベースで、**Windows と Mac 両プラットフォーム対応**を想定する。

現状は URP テンプレートの初期状態で、独自スクリプトは未追加(`Assets/TutorialInfo/Scripts/Readme.cs` および `ReadmeEditor.cs` は Unity テンプレート由来のチュートリアル表示用で、ゲーム機能ではない)。

アーキテクチャは未確立。最初のスクリプト・シェーダーを追加する際は以下のディレクトリ構成をベースとし、必要に応じてユーザーと合意の上で調整すること:

- `Assets/Scripts/` — Viewer の UI / カメラ操作 / シェーダー切替ロジック等の C# コード
- `Assets/Shaders/` — `.shader` / ShaderGraph アセット
- `Assets/Materials/` — シェーダーごとのマテリアル
- `Assets/Models/` または `Assets/Prefabs/` — プレビュー対象オブジェクト

## クロスプラットフォーム (Windows / Mac) 対応上の注意

- ファイル I/O や外部プロセス起動を行う際は `Path.Combine` / `Application.persistentDataPath` 等のクロスプラットフォーム API を使用し、パス区切り文字 (`\` / `/`) や改行コードをハードコードしない
- Mac 向けビルドは `StandaloneOSX` (Apple Silicon / Intel / Universal) を、Windows 向けは `StandaloneWindows64` をターゲットにする想定。`File > Build Settings` でターゲット切替時に Library 再インポートが走るため時間がかかる
- シェーダーは URP 互換のものを使用すること。Built-in RP 用シェーダーは URP では動かないか、ピンク色 (シェーダーエラー) で表示される
- Mac は Metal、Windows は Direct3D 11/12 がデフォルトのグラフィクス API。プラットフォーム固有の HLSL 機能を使う際は両方で動作確認する

## 重要な環境情報

- **Unity バージョン**: 2022.3.62f3 (C# 9 相当 — ファイルスコープ namespace は使用不可)
- **レンダーパイプライン**: URP (`com.unity.render-pipelines.universal` 14.0.12)
  - URP プロファイルは `Assets/Settings/` に Performant / Balanced / HighFidelity の 3 段階が用意されている
- **アセンブリ構成**: `.asmdef` 未配置 → 全スクリプトが `Assembly-CSharp` (ランタイム) または `Assembly-CSharp-Editor` (Editor) に入る。コード規模が増える前に `.asmdef` 分割を提案すべき
- **テストフレームワーク**: `com.unity.test-framework` 1.1.33 はインストール済みだがテストアセンブリ未作成
- **エントリシーン**: `Assets/Scenes/Main.unity`

## ビルド・テスト・実行

GUI 操作が基本で CLI スクリプトは未整備:

- **ビルド**: Unity Editor を開き、`File > Build Settings` から実行
- **再生**: Unity Editor の Play モード
- **テスト**: Unity Editor の `Window > General > Test Runner` (Edit Mode / Play Mode)。CLI 実行が必要な場合は `Unity.exe -batchmode -runTests -projectPath . -testResults results.xml -testPlatform EditMode` 等を使用

C# のコンパイルエラー確認は Unity の Console を介する必要がある。後述の UnityMCP が利用可能ならそれ経由が最速。

## UnityMCP について

`com.coplaydev.unity-mcp` が `Packages/manifest.json` に導入済み。Unity Editor 操作は UnityMCP ツール・リソースを優先的に使用する:

- **Editor 状態確認**: `mcpforunity://editor/state` リソースで Play モード・コンパイル状態・ドメインリロード状況を確認
- **Console ログ取得**: `read_console` ツールでエラー・警告を取得 (スクリプト変更後は必ずチェック)
- **シーン操作**: `manage_scene` (CRUD)、`find_gameobjects` (検索)、`manage_gameobject` (個別操作)
- **アセット操作**: `manage_asset`、`manage_material`、`manage_shader` 等
- **スクリプト変更フロー**: スクリプトを作成・編集したら `read_console` でコンパイルエラー確認 → エラーがなければ次の操作へ進む

複数 Unity インスタンスが起動中の場合は `set_active_instance` でターゲットを固定する (本プロジェクトは `ShaderScope`)。

## コーディング規約

ユーザーのグローバル CLAUDE.md に Unity C# プロジェクト共通の規約 (命名規約、`#region` 構造、ログレベル、Git ワークフロー、`.meta` 取り扱い等) が定義されている。**このプロジェクトでも全面的にこれらを遵守する**。重複は避けるためここでは詳述しない。

特にこのプロジェクトに紐づく注意点:

- Unity 2022.3 = C# 9 のため、`namespace Foo;` (ファイルスコープ namespace) は使えない。`namespace Foo { ... }` のブロック形式を使う(なお、グローバル規約では「namespace は使用しない」方針)
- 1 ファイル 1 MonoBehaviour、ファイル名 = クラス名
- Inspector 公開は `[SerializeField] private` を使う(`public` フィールドは作らない)
- `Library/`, `Temp/`, `Logs/`, `UserSettings/`, `*.csproj`, `*.sln`, `.vscode/` はコミット対象外

## Git ワークフロー

グローバル CLAUDE.md に共通の Git ワークフロー (コミットメッセージのプレフィックス、コミット粒度、ユーザー確認フロー等) が定義されているのでそれに従う。本プロジェクト固有の運用は以下:

- **ブランチ運用**: `main` はリリース安定版、`dev` は開発統合用ブランチ。開発中の作業は `feature/*` 等の作業ブランチから `dev` に PR を出す。`main` を直接ベースにしてよいのは `dev` → `main` のリリース統合や hotfix 等、明示的な指示があるときのみ
- **PR 作成時のデフォルトベース**: `gh pr create` の際は明示的に `--base dev` を指定する (リポジトリ既定ブランチが `main` のため省略すると `main` がベースになるので注意)

## ファイルの追加・リネーム・削除

Unity プロジェクトでは `.meta` ファイルとペアで管理する必要がある。スクリプト追加時は `.meta` が Unity により自動生成されるが、`git mv` や手動でのファイル操作を行う場合は対応する `.meta` ファイルも必ず同様に処理されているか確認すること。漏れがあればユーザーに報告する。
