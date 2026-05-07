# ShaderScope

Shader の種類を選択するとそれが反映されたオブジェクトを確認できる、シェーダー Viewer アプリです。

## 特徴

- Shader をリストから選択 → 対応するマテリアルがプレビュー対象オブジェクトに即時反映される
- Windows / Mac 両プラットフォーム対応
- URP (Universal Render Pipeline) ベース

## 動作環境 / 開発環境

| 項目 | バージョン |
|------|-----------|
| Unity | **2022.3.62f3** (LTS) |
| Render Pipeline | URP 14.0.12 |
| 対応プラットフォーム | Windows (Standalone 64bit) / macOS (Apple Silicon / Intel) |

## セットアップ

1. Unity Hub から Unity **2022.3.62f3** をインストールする
2. このリポジトリをクローン
   ```
   git clone <repository-url>
   ```
3. Unity Hub で「Add project from disk」からクローンしたディレクトリを開く
4. 初回起動時は Library フォルダの再生成が走るため数分かかる場合がある

## 実行方法

### Editor で再生する

Unity Editor を開き、`Assets/Scenes/Main.unity` を開いて Play ボタンを押す。

### ビルドする

`File > Build Settings` を開き、ターゲットプラットフォームを切り替えてビルド:

- **Windows**: `Windows` を選択し `Architecture: x86_64` でビルド
- **macOS**: `Mac OS X` を選択し `Architecture: Apple Silicon` / `Intel 64-bit` / `Intel 64-bit + Apple Silicon` のいずれかを選択

## ディレクトリ構成 (予定)

```
Assets/
├── Scenes/         Viewer のメインシーン (Main.unity)
├── Scripts/        UI / カメラ操作 / シェーダー切替ロジック (C#)
├── Shaders/        .shader / ShaderGraph アセット
├── Materials/      シェーダーごとのマテリアル
├── Models/         プレビュー対象の 3D モデル
├── UI/             UI 用に自動生成される sprite 等
└── Settings/       URP プロファイル (Performant / Balanced / HighFidelity)
```

## 開発規約

コーディング規約・命名規約・Git ワークフロー等は `CLAUDE.md` を参照してください。
