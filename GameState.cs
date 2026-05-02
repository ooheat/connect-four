using System;

public class GameState
{
    public enum WinState { None, Player1_Wins, Player2_Wins, Tie }

    public int PlayerTurn { get; private set; } = 1;
    public int CurrentTurn { get; private set; } = 0;

    // 7개의 열, 6개의 행을 가진 보드 (0은 빈칸, 1은 플레이어1, 2는 플레이어2)
    private int[,] board = new int[7, 6];

    public void ResetBoard()
    {
        board = new int[7, 6];
        PlayerTurn = 1;
        CurrentTurn = 0;
    }

    public byte PlayPiece(byte col)
    {
        if (CheckForWin() != WinState.None)
            throw new ArgumentException("게임이 이미 종료되었습니다.");

        // 맨 아래 행(0)부터 위로 올라가며 빈자리 찾기
        for (byte row = 0; row < 6; row++)
        {
            if (board[col, row] == 0)
            {
                board[col, row] = PlayerTurn;
                CurrentTurn++;

                // CSS의 drop1 ~ drop6 애니메이션에 맞추기 위한 계산
                byte landingRow = (byte)(6 - row);

                // 턴 넘기기
                PlayerTurn = PlayerTurn == 1 ? 2 : 1;
                return landingRow;
            }
        }
        throw new ArgumentException("이 열은 이미 가득 찼습니다.");
    }

    public WinState CheckForWin()
    {
        for (int c = 0; c < 7; c++)
        {
            for (int r = 0; r < 6; r++)
            {
                int player = board[c, r];
                if (player == 0) continue;

                if (c + 3 < 7 && player == board[c + 1, r] && player == board[c + 2, r] && player == board[c + 3, r])
                    return player == 1 ? WinState.Player1_Wins : WinState.Player2_Wins;

                if (r + 3 < 6 && player == board[c, r + 1] && player == board[c, r + 2] && player == board[c, r + 3])
                    return player == 1 ? WinState.Player1_Wins : WinState.Player2_Wins;

                if (c + 3 < 7 && r + 3 < 6 && player == board[c + 1, r + 1] && player == board[c + 2, r + 2] && player == board[c + 3, r + 3])
                    return player == 1 ? WinState.Player1_Wins : WinState.Player2_Wins;

                if (c + 3 < 7 && r - 3 >= 0 && player == board[c + 1, r - 1] && player == board[c + 2, r - 2] && player == board[c + 3, r - 3])
                    return player == 1 ? WinState.Player1_Wins : WinState.Player2_Wins;
            }
        }

        if (CurrentTurn == 42) return WinState.Tie;
        return WinState.None;
    }
}
