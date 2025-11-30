# TLUDUS

A strategic twist on classic Tic-Tac-Toe where you can push rows and columns to create winning combinations. Battle against an AI opponent through 8 progressive rounds of increasing difficulty.

## Overview

TLUDUS (a variant spelling of "Tic-Tac-Toe") is a competitive puzzle game that adds a unique mechanic to the traditional game - the ability to slide entire rows and columns. Face off against a robot opponent that becomes increasingly challenging as you progress through rounds.

## Features

- **Push Mechanic**: Slide entire rows or columns to shift pieces and create strategic advantages
- **8 Progressive Rounds**: Each round increases in difficulty with different AI strategies
- **Dynamic AI Behavior**:
  - Round 1: Random moves
  - Rounds 2-4: Aggressive offense
  - Rounds 5-7: Defensive tactics
  - Round 8: Hybrid strategy with push mechanics
- **Robot Dialogue System**: The AI opponent reacts to victories and defeats with themed responses
- **Permadeath System**: Lose any round and restart from Round 1
- **Victory Condition**: Clear all 8 rounds to achieve final victory

## Gameplay

### Basic Rules
- Play standard Tic-Tac-Toe against the AI opponent
- Alternating turns between player and computer
- First player is randomly determined each round

### Push Mechanic
- Click arrow buttons around the board to push entire rows or columns
- Pushed pieces shift in the direction of the arrow
- The last space becomes empty after a push
- Use strategically to create winning lines or block opponent victories

### Round System
- Win a round to advance to the next level
- Lose any round and restart from Round 1
- Successfully complete Round 8 to win the game

## Technical Details

**Built with Unity**

### Key Scripts
- `TicTacToeScript.cs`: Main game logic including AI, board state, and win conditions
- `SceneManager.cs`: Scene navigation and menu system
- `ExitHowToPlayScene.cs`: Tutorial scene management

### AI Implementation
The AI uses different strategies per round:
- **Random**: Pure random selection
- **Offensive**: Prioritizes winning moves
- **Defensive**: Blocks player winning moves
- **Hybrid**: Evaluates push mechanics with scoring system (Round 8 only)

## Installation

1. Clone this repository
2. Open the project in Unity (version compatible with your Unity installation)
3. Open the `IntroScene` to start
4. Build and run for your target platform

## Controls

- **Mouse**: Click tiles to place your mark
- **Arrow Buttons**: Click arrows around the board to push rows/columns
- **ESC**: Pause menu during gameplay

## Development

This project was created as a campus game hackathon competition entry, 

## Contact

For suggestions or feedback: ditto_eevee@icloud.com
****
# TLUDUS

行と列を押し出すことができる、戦略的なマルバツゲーム。段階的に難易度が上がる8ラウンドを通じてAI対戦相手と戦います。

## 概要

TLUDUS(「Tic-Tac-Toe」の変形綴り)は、伝統的なゲームにユニークなメカニクスを追加した対戦型パズルゲームです - 行と列全体をスライドさせる能力。ラウンドを進めるごとに難易度が上がるロボット対戦相手と対決します。

## 特徴

- **プッシュメカニクス**: 行または列全体をスライドさせてピースをシフトし、戦略的な優位性を作り出す
- **8つの段階的ラウンド**: 各ラウンドで異なるAI戦略により難易度が上昇
- **動的なAI行動**:
  - ラウンド1: ランダム
  - ラウンド2-4: 攻撃的なAI
  - ラウンド5-7: 防御的な戦術
  - ラウンド8: プッシュメカニクスを含むハイブリッド戦略
- **ロボット対話システム**: AI対戦相手が勝利と敗北にテーマ性のある反応を示す
- **パーマデスシステム**: どのラウンドでも負けるとラウンド1から再スタート
- **勝利条件**: 8ラウンドすべてをクリアして最終勝利を達成

## ゲームプレイ

### 基本ルール
- AI対戦相手と標準的なマルバツゲームをプレイ
- プレイヤーとコンピューターが交互にターンを取る
- 各ラウンドで先攻はランダムに決定

### プッシュメカニズム
- ボード周りの矢印ボタンをクリックして行または列全体をプッシュ
- プッシュされたピースは矢印の方向にシフト
- プッシュ後、最後のスペースが空になる
- 勝利ラインを作成したり、相手の勝利をブロックするために戦略的に使用

### ラウンドシステム
- ラウンドに勝利すると次のレベルに進む
- どのラウンドでも負けるとラウンド1から再スタート
- ラウンド8を成功裏に完了するとゲームに勝利

## 技術詳細

**Unityで構築**

### 主要スクリプト
- `TicTacToeScript.cs`: AI、ボード状態、勝利条件を含むメインゲームロジック
- `SceneManager.cs`: シーンナビゲーションとメニューシステム
- `ExitHowToPlayScene.cs`: チュートリアルシーン管理

### AI実装
AIはラウンドごとに異なる戦略を使用:
- **ランダム**: 純粋なランダム選択
- **攻撃的**: 勝利手を優先
- **防御的**: プレイヤーの勝利手をブロック
- **ハイブリッド**: スコアリングシステムでプッシュメカニクスを評価(ラウンド8のみ)

## インストール

1. このリポジトリをクローン
2. Unity(お使いのUnityインストールと互換性のあるバージョン)でプロジェクトを開く
3. `IntroScene`を開いて開始
4. ターゲットプラットフォーム用にビルドして実行

## 操作方法

- **マウス**: タイルをクリックしてマークを配置
- **矢印ボタン**: ボード周りの矢印をクリックして行/列をプッシュ
- **ESC**: ゲームプレイ中の一時停止メニュー

## 開発

このプロジェクトは、クラシックゲームにおける革新的なメカニクスを探求するキャンパスコンペティションエントリーとして作成されました。

## 連絡先

提案やフィードバック: ditto_eevee@icloud.com

