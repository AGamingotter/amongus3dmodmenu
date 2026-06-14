using System;
using System.Collections.Generic;
using System.Reflection;
using Il2CppFusion;
using Il2CppSG.Airlock;
using Il2CppSG.Airlock.Network;
using Il2CppSG.Airlock.Roles;
using Il2CppSG.Airlock.XR;
using MelonLoader;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AU3DCheats
{
	// Token: 0x02000002 RID: 2
	public class CheatMod : MelonMod
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static Texture2D MakeTex(int w, int h, Color col)
		{
			Color[] array = new Color[w * h];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = col;
			}
			Texture2D texture2D = new Texture2D(w, h);
			texture2D.SetPixels(array);
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020A4 File Offset: 0x000002A4
		private static Texture2D MakeGrad(int w, int h, Color a, Color b)
		{
			Color[] array = new Color[w * h];
			for (int i = 0; i < h; i++)
			{
				for (int j = 0; j < w; j++)
				{
					array[i * w + j] = Color.Lerp(a, b, (float)j / (float)(w - 1));
				}
			}
			Texture2D texture2D = new Texture2D(w, h);
			texture2D.SetPixels(array);
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000211B File Offset: 0x0000031B
		private static void DrawRect(float x, float y, float w, float h, Color col)
		{
			GUI.color = col;
			GUI.DrawTexture(new Rect(x, y, w, h), Texture2D.whiteTexture);
			GUI.color = Color.white;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002148 File Offset: 0x00000348
		private static void DrawPanel(float x, float y, float w, float h, Color fill, Color border, float r = 10f, float bw = 1.5f)
		{
			CheatMod.DrawRect(x + r, y, w - r * 2f, h, fill);
			CheatMod.DrawRect(x, y + r, w, h - r * 2f, fill);
			for (int i = 0; i < 4; i++)
			{
				CheatMod.DrawCorner(x, y, w, h, r, fill, i);
			}
			GUI.color = border;
			GUI.DrawTexture(new Rect(x + r, y, w - r * 2f, bw), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(x + r, y + h - bw, w - r * 2f, bw), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(x, y + r, bw, h - r * 2f), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(x + w - bw, y + r, bw, h - r * 2f), Texture2D.whiteTexture);
			GUI.color = Color.white;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002248 File Offset: 0x00000448
		private static void DrawCorner(float px, float py, float w, float h, float r, Color col, int quad)
		{
			float num = (quad == 0 || quad == 2) ? (px + r) : (px + w - r);
			float num2 = (quad == 0 || quad == 1) ? (py + r) : (py + h - r);
			GUI.color = col;
			for (int i = 0; i < 10; i++)
			{
				float num3 = (float)quad * 3.1415927f * 0.5f + (float)i / 10f * 3.1415927f * 0.5f;
				float num4 = (float)quad * 3.1415927f * 0.5f + (float)(i + 1) / 10f * 3.1415927f * 0.5f;
				float num5 = Mathf.Cos(num3) * r;
				float num6 = Mathf.Sin(num3) * r;
				float num7 = Mathf.Cos(num4) * r;
				float num8 = Mathf.Sin(num4) * r;
				GUI.DrawTexture(new Rect(num + Mathf.Min(num5, num7), num2 - Mathf.Max(num6, num8), Mathf.Abs(num5 - num7) + 1f, Mathf.Abs(num6 - num8) + 1f), Texture2D.whiteTexture);
			}
			GUI.color = Color.white;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002371 File Offset: 0x00000571
		private static void DrawGlow(float x, float y, float w, Color col)
		{
			col.a = 0.15f;
			CheatMod.DrawRect(x, y - 2f, w, 5f, col);
			col.a = 0.6f;
			CheatMod.DrawRect(x, y, w, 1f, col);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000023B0 File Offset: 0x000005B0
		private bool AAAButton(Rect rect, string label, GUIStyle style, string id)
		{
			float num = 0f;
			this._btnClickTimers.TryGetValue(id, out num);
			Rect rect2 = rect;
			bool flag = num > 0f;
			if (flag)
			{
				float num2 = 1f - num / 0.12f;
				float num3 = 1f - Mathf.Sin(num2 * 3.1415927f) * 0.06f;
				float num4 = rect.width * (1f - num3);
				float num5 = rect.height * (1f - num3);
				rect2..ctor(rect.x + num4 / 2f, rect.y + num5 / 2f, rect.width - num4, rect.height - num5);
			}
			bool flag2 = GUI.Button(rect2, label, style);
			Vector2 mousePosition = Event.current.mousePosition;
			bool flag3 = rect.Contains(mousePosition);
			bool flag4 = flag3 && Event.current.type == 7;
			if (flag4)
			{
				CheatMod.DrawRect(rect2.x, rect2.y, rect2.width, rect2.height, new Color(1f, 1f, 1f, 0.08f));
				CheatMod.DrawRect(rect2.x, rect2.y, rect2.width * 0.35f, rect2.height, new Color(1f, 1f, 1f, 0.07f));
				CheatMod.DrawRect(rect2.x + 4f, rect2.y + 1f, rect2.width - 8f, 1f, new Color(1f, 1f, 1f, 0.3f));
			}
			bool flag5 = flag2;
			if (flag5)
			{
				this._btnClickTimers[id] = 0.12f;
			}
			return flag2;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002598 File Offset: 0x00000798
		private bool AAAToggle(Rect rect, bool state, string labelOn, string labelOff, string id)
		{
			GUIStyle style = state ? CheatMod.btnToggleOn : CheatMod.btnToggleOff;
			string label = "● " + (state ? labelOn : labelOff);
			bool flag = this.AAAButton(rect, label, style, id);
			bool flag2 = Event.current.type == 7;
			if (flag2)
			{
				Color col = state ? new Color(0.15f, 1f, 0.35f) : new Color(1f, 0.18f, 0.18f);
				float x = rect.x + 10f;
				float y = rect.y + rect.height / 2f - 5f;
				CheatMod.DrawRect(x, y, 10f, 10f, col);
			}
			bool flag3 = flag;
			bool result;
			if (flag3)
			{
				result = !state;
			}
			else
			{
				result = state;
			}
			return result;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002670 File Offset: 0x00000870
		private static GUIStyle MakeBtn(Color top, Color bot, Color hTop, Color hBot)
		{
			GUIStyle guistyle = new GUIStyle(GUI.skin.button)
			{
				fontSize = 13,
				fontStyle = 1
			};
			guistyle.normal.background = CheatMod.MakeGrad(4, 2, top, bot);
			guistyle.hover.background = CheatMod.MakeGrad(4, 2, hTop, hBot);
			guistyle.active.background = CheatMod.MakeGrad(4, 2, bot, top);
			guistyle.normal.textColor = Color.white;
			guistyle.hover.textColor = Color.white;
			guistyle.active.textColor = new Color(0.8f, 0.8f, 0.8f);
			guistyle.border = new RectOffset(6, 6, 6, 6);
			guistyle.padding = new RectOffset(22, 8, 6, 6);
			return guistyle;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002748 File Offset: 0x00000948
		private static GUIStyle MakeBtnNoDot(Color top, Color bot, Color hTop, Color hBot)
		{
			GUIStyle guistyle = CheatMod.MakeBtn(top, bot, hTop, hBot);
			guistyle.padding = new RectOffset(8, 8, 6, 6);
			return guistyle;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002778 File Offset: 0x00000978
		private void InitStyles()
		{
			bool flag = CheatMod.stylesInit;
			if (!flag)
			{
				CheatMod.stylesInit = true;
				CheatMod.titleStyle = new GUIStyle(GUI.skin.label)
				{
					fontSize = 20,
					fontStyle = 1,
					alignment = 4
				};
				CheatMod.titleStyle.normal.textColor = new Color(1f, 0.92f, 0.92f);
				CheatMod.subtitleStyle = new GUIStyle(GUI.skin.label)
				{
					fontSize = 10,
					alignment = 4
				};
				CheatMod.subtitleStyle.normal.textColor = new Color(1f, 0.4f, 0.4f, 0.6f);
				CheatMod.tabActiveStyle = new GUIStyle(GUI.skin.button)
				{
					fontSize = 11,
					fontStyle = 1
				};
				CheatMod.tabActiveStyle.normal.background = CheatMod.MakeGrad(4, 2, new Color(0.85f, 0.1f, 0.1f), new Color(0.55f, 0.04f, 0.04f));
				CheatMod.tabActiveStyle.hover.background = CheatMod.MakeGrad(4, 2, new Color(1f, 0.18f, 0.18f), new Color(0.68f, 0.06f, 0.06f));
				CheatMod.tabActiveStyle.normal.textColor = Color.white;
				CheatMod.tabActiveStyle.hover.textColor = Color.white;
				CheatMod.tabStyle = new GUIStyle(GUI.skin.button)
				{
					fontSize = 11
				};
				CheatMod.tabStyle.normal.background = CheatMod.MakeTex(2, 2, new Color(0.14f, 0.04f, 0.04f, 1f));
				CheatMod.tabStyle.hover.background = CheatMod.MakeTex(2, 2, new Color(0.24f, 0.08f, 0.08f, 1f));
				CheatMod.tabStyle.normal.textColor = new Color(1f, 0.55f, 0.55f);
				CheatMod.tabStyle.hover.textColor = Color.white;
				CheatMod.btnToggleOn = CheatMod.MakeBtn(new Color(0.08f, 0.72f, 0.22f), new Color(0.04f, 0.46f, 0.14f), new Color(0.12f, 0.85f, 0.28f), new Color(0.06f, 0.55f, 0.18f));
				CheatMod.btnToggleOff = CheatMod.MakeBtn(new Color(0.55f, 0.06f, 0.06f), new Color(0.32f, 0.03f, 0.03f), new Color(0.72f, 0.1f, 0.1f), new Color(0.44f, 0.05f, 0.05f));
				CheatMod.btnAction = CheatMod.MakeBtnNoDot(new Color(0.82f, 0.08f, 0.08f), new Color(0.52f, 0.04f, 0.04f), new Color(1f, 0.14f, 0.14f), new Color(0.66f, 0.06f, 0.06f));
				CheatMod.btnGold = CheatMod.MakeBtnNoDot(new Color(0.8f, 0.56f, 0.04f), new Color(0.52f, 0.36f, 0.02f), new Color(0.96f, 0.7f, 0.08f), new Color(0.64f, 0.44f, 0.04f));
				CheatMod.btnDanger = CheatMod.MakeBtnNoDot(new Color(0.4f, 0.02f, 0.02f), new Color(0.22f, 0.01f, 0.01f), new Color(0.56f, 0.04f, 0.04f), new Color(0.3f, 0.02f, 0.02f));
				CheatMod.labelStyle = new GUIStyle(GUI.skin.label)
				{
					fontSize = 13
				};
				CheatMod.labelStyle.normal.textColor = new Color(1f, 0.85f, 0.85f);
				CheatMod.sectionStyle = new GUIStyle(GUI.skin.label)
				{
					fontSize = 10,
					fontStyle = 1
				};
				CheatMod.sectionStyle.normal.textColor = new Color(1f, 0.38f, 0.38f, 0.9f);
				CheatMod.valueStyle = new GUIStyle(GUI.skin.label)
				{
					fontSize = 13,
					fontStyle = 1
				};
				CheatMod.valueStyle.normal.textColor = new Color(1f, 0.6f, 0.6f);
				CheatMod.footerStyle = new GUIStyle(GUI.skin.label)
				{
					fontSize = 10,
					alignment = 4
				};
				CheatMod.footerStyle.normal.textColor = new Color(0.72f, 0.28f, 0.28f, 0.55f);
				CheatMod.hintStyle = new GUIStyle(GUI.skin.label)
				{
					fontSize = 11
				};
				CheatMod.hintStyle.normal.textColor = new Color(1f, 1f, 1f, 0.28f);
				CheatMod.badgeStyle = new GUIStyle(GUI.skin.label)
				{
					fontSize = 10,
					fontStyle = 1,
					alignment = 5
				};
				CheatMod.badgeStyle.normal.textColor = Color.white;
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002D2C File Offset: 0x00000F2C
		private Transform GetMyRigRoot()
		{
			bool flag = Camera.main == null;
			Transform result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Transform transform = Camera.main.transform;
				while (transform.parent != null)
				{
					transform = transform.parent;
				}
				result = transform;
			}
			return result;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002D78 File Offset: 0x00000F78
		private void RefreshLocalPlayer()
		{
			try
			{
				NetworkedLocomotionPlayer networkedLocomotionPlayer = null;
				PlayerState playerState = null;
				foreach (NetworkedLocomotionPlayer networkedLocomotionPlayer2 in Object.FindObjectsOfType<NetworkedLocomotionPlayer>())
				{
					bool flag = networkedLocomotionPlayer2 == null;
					if (!flag)
					{
						bool flag2 = !networkedLocomotionPlayer2.LocalPlayerSpawned;
						if (!flag2)
						{
							bool flag3 = !networkedLocomotionPlayer2.HasSpawnedIn;
							if (!flag3)
							{
								PlayerState playerState2 = null;
								try
								{
									playerState2 = networkedLocomotionPlayer2.PState;
								}
								catch
								{
								}
								bool flag4 = playerState2 == null;
								if (flag4)
								{
									foreach (PlayerState playerState3 in Object.FindObjectsOfType<PlayerState>())
									{
										bool flag5 = playerState3 == null || playerState3.IsDummy || playerState3.IsBackup;
										if (!flag5)
										{
											bool flag6 = playerState3.LocomotionPlayer == null;
											if (!flag6)
											{
												bool flag7 = playerState3.LocomotionPlayer.GetInstanceID() == networkedLocomotionPlayer2.GetInstanceID();
												if (flag7)
												{
													playerState2 = playerState3;
													break;
												}
											}
										}
									}
								}
								bool flag8 = playerState2 == null || playerState2.IsDummy || playerState2.IsBackup;
								if (!flag8)
								{
									bool flag9 = false;
									try
									{
										flag9 = networkedLocomotionPlayer2.Object.HasInputAuthority;
									}
									catch
									{
									}
									bool flag10 = networkedLocomotionPlayer == null || flag9;
									if (flag10)
									{
										networkedLocomotionPlayer = networkedLocomotionPlayer2;
										playerState = playerState2;
										bool flag11 = flag9;
										if (flag11)
										{
											break;
										}
									}
								}
							}
						}
					}
				}
				bool flag12 = networkedLocomotionPlayer != null && playerState != null;
				if (flag12)
				{
					bool flag13 = this._cachedLocalNLP != networkedLocomotionPlayer || this._cachedLocalPlayer != playerState;
					if (flag13)
					{
						MelonLogger.Msg("[Tr.Rism] Local locked via LocalPlayerSpawned: " + string.Format("{0} (id={1})", playerState.DisplayName, playerState.PlayerId));
					}
					this._cachedLocalNLP = networkedLocomotionPlayer;
					this._cachedLocalPlayer = playerState;
				}
				else
				{
					bool flag14 = Camera.main != null;
					if (flag14)
					{
						Vector3 position = Camera.main.transform.position;
						NetworkedLocomotionPlayer networkedLocomotionPlayer3 = null;
						PlayerState playerState4 = null;
						float num = float.MaxValue;
						foreach (NetworkedLocomotionPlayer networkedLocomotionPlayer4 in Object.FindObjectsOfType<NetworkedLocomotionPlayer>())
						{
							bool flag15 = networkedLocomotionPlayer4 == null || !networkedLocomotionPlayer4.HasSpawnedIn;
							if (!flag15)
							{
								float num2 = Vector3.Distance(position, networkedLocomotionPlayer4.transform.position);
								bool flag16 = num2 < num;
								if (flag16)
								{
									PlayerState playerState5 = null;
									try
									{
										playerState5 = networkedLocomotionPlayer4.PState;
									}
									catch
									{
									}
									bool flag17 = playerState5 != null && !playerState5.IsDummy && !playerState5.IsBackup;
									if (flag17)
									{
										num = num2;
										networkedLocomotionPlayer3 = networkedLocomotionPlayer4;
										playerState4 = playerState5;
									}
								}
							}
						}
						bool flag18 = networkedLocomotionPlayer3 != null && num < 2.5f;
						if (flag18)
						{
							bool flag19 = this._cachedLocalNLP != networkedLocomotionPlayer3;
							if (flag19)
							{
								MelonLogger.Msg("[Tr.Rism] Local locked (cam fallback): " + string.Format("{0} dist={1:0.00}m", playerState4.DisplayName, num));
							}
							this._cachedLocalNLP = networkedLocomotionPlayer3;
							this._cachedLocalPlayer = playerState4;
							return;
						}
					}
					this._cachedLocalNLP = null;
					this._cachedLocalPlayer = null;
				}
			}
			catch (Exception ex)
			{
				MelonLogger.Msg("[Tr.Rism] RefreshLocalPlayer error: " + ex.Message);
				this._cachedLocalNLP = null;
				this._cachedLocalPlayer = null;
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000031D0 File Offset: 0x000013D0
		private void RefreshAllPlayers()
		{
			this.cachedAllPlayers.Clear();
			try
			{
				foreach (PlayerState playerState in Object.FindObjectsOfType<PlayerState>())
				{
					bool flag = playerState == null || playerState.IsDummy || playerState.IsBackup;
					if (!flag)
					{
						bool flag2 = string.IsNullOrEmpty(playerState.DisplayName);
						if (!flag2)
						{
							this.cachedAllPlayers.Add(playerState);
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000327C File Offset: 0x0000147C
		private void RefreshDebugInfo()
		{
			try
			{
				bool flag = this._cachedLocalPlayer == null;
				if (flag)
				{
					this.RefreshLocalPlayer();
				}
				bool flag2 = this._cachedLocalNLP != null && this._cachedLocalPlayer != null;
				if (flag2)
				{
					CheatMod.debugName = (string.IsNullOrEmpty(this._cachedLocalPlayer.DisplayName) ? "(no name)" : this._cachedLocalPlayer.DisplayName);
					CheatMod.debugId = this._cachedLocalPlayer.PlayerId.ToString();
					try
					{
						CheatMod.debugPos = this._cachedLocalNLP.RigidbodyPosition;
					}
					catch
					{
						CheatMod.debugPos = this._cachedLocalNLP.transform.position;
					}
					try
					{
						NetworkRunner networkRunner = Object.FindObjectOfType<NetworkRunner>();
						bool flag3 = networkRunner != null && networkRunner.IsRunning;
						if (flag3)
						{
							float num = (float)networkRunner.GetPlayerRtt(this._cachedLocalPlayer.Object.InputAuthority);
							CheatMod.debugPing = Mathf.RoundToInt(num * 1000f).ToString() + " ms";
						}
						else
						{
							CheatMod.debugPing = "offline";
						}
					}
					catch
					{
						CheatMod.debugPing = "N/A";
					}
				}
				else
				{
					CheatMod.debugName = "(searching…)";
					CheatMod.debugId = "N/A";
					CheatMod.debugPing = "N/A";
					CheatMod.debugPos = Vector3.zero;
				}
			}
			catch (Exception ex)
			{
				MelonLogger.Msg("[Tr.Rism] RefreshDebugInfo: " + ex.Message);
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000344C File Offset: 0x0000164C
		private void FindKillBehaviour()
		{
			try
			{
				bool flag = this._cachedLocalNLP != null;
				if (flag)
				{
					try
					{
						NetworkedKillBehaviour componentInChildren = this._cachedLocalNLP.GetComponentInChildren<NetworkedKillBehaviour>();
						bool flag2 = componentInChildren != null;
						if (flag2)
						{
							this._cachedKillBehaviour = componentInChildren;
							return;
						}
					}
					catch
					{
					}
				}
				foreach (NetworkedKillBehaviour networkedKillBehaviour in Object.FindObjectsOfType<NetworkedKillBehaviour>())
				{
					bool flag3 = networkedKillBehaviour != null;
					if (flag3)
					{
						this._cachedKillBehaviour = networkedKillBehaviour;
						break;
					}
				}
			}
			catch (Exception ex)
			{
				MelonLogger.Msg("[Tr.Rism] FindKillBehaviour: " + ex.Message);
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00003528 File Offset: 0x00001728
		private void ApplyNoKillCD()
		{
			bool flag = this._cachedLocalPlayer != null;
			if (flag)
			{
				try
				{
					this._cachedLocalPlayer.ActionCooldownRemaining = 0;
				}
				catch
				{
				}
			}
			bool flag2 = this._cachedKillBehaviour == null;
			if (!flag2)
			{
				try
				{
					this._cachedKillBehaviour.EndCooldown();
					Type type = this._cachedKillBehaviour.GetType();
					this.ZeroField(type, "_actionCooldownLeft");
					this.ZeroField(type, "_actionCooldown");
					this.ZeroField(type, "_actionCooldownTime");
					this.ZeroField(type, "_actionCooldownOffset");
					this.ZeroField(type, "_actionCooldownTotalOffset");
					this.ZeroField(type, "_cachedActionCooldown");
				}
				catch
				{
					this._cachedKillBehaviour = null;
				}
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00003604 File Offset: 0x00001804
		private void ZeroField(Type t, string name)
		{
			FieldInfo field = t.GetField(name, CheatMod._priv);
			bool flag = field != null;
			if (flag)
			{
				field.SetValue(this._cachedKillBehaviour, 0f);
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00003644 File Offset: 0x00001844
		private void ApplyInvisible()
		{
			bool flag = CheatMod.invisibleOn && !this._invisibleWasOn;
			bool flag2 = !CheatMod.invisibleOn && this._invisibleWasOn;
			bool flag3 = flag;
			if (flag3)
			{
				bool flag4 = this._cachedLocalNLP == null;
				if (flag4)
				{
					this._invisibleWasOn = CheatMod.invisibleOn;
					return;
				}
				try
				{
					this._invisibleSavedPosition = this._cachedLocalNLP.RigidbodyPosition;
				}
				catch
				{
					this._invisibleSavedPosition = this._cachedLocalNLP.transform.position;
				}
				bool flag5 = this._invisibleSavedPosition.y < -10f;
				if (flag5)
				{
					MelonLogger.Msg(string.Format("[Tr.Rism] Invisible: skipping save, Y already at {0:0.00}", this._invisibleSavedPosition.y));
					this._invisibleWasOn = CheatMod.invisibleOn;
					return;
				}
				this._invisiblePositionSaved = true;
				Vector3 vector;
				vector..ctor(this._invisibleSavedPosition.x, -50f, this._invisibleSavedPosition.z);
				this.SinkNLP(vector);
				MelonLogger.Msg(string.Format("[Tr.Rism] Invisible ON — saved {0}, sinking to {1}", this._invisibleSavedPosition, vector));
			}
			else
			{
				bool flag6 = flag2;
				if (flag6)
				{
					bool invisiblePositionSaved = this._invisiblePositionSaved;
					if (invisiblePositionSaved)
					{
						this.SinkNLP(this._invisibleSavedPosition);
						MelonLogger.Msg(string.Format("[Tr.Rism] Invisible OFF — restored to {0}", this._invisibleSavedPosition));
						this._invisiblePositionSaved = false;
					}
				}
				else
				{
					bool flag7 = CheatMod.invisibleOn && this._cachedLocalNLP != null;
					if (flag7)
					{
						float y;
						try
						{
							y = this._cachedLocalNLP.RigidbodyPosition.y;
						}
						catch
						{
							y = this._cachedLocalNLP.transform.position.y;
						}
						bool flag8 = y > -45f;
						if (flag8)
						{
							Vector3 dest;
							dest..ctor(this._invisibleSavedPosition.x, -50f, this._invisibleSavedPosition.z);
							this.SinkNLP(dest);
						}
					}
				}
			}
			this._invisibleWasOn = CheatMod.invisibleOn;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00003874 File Offset: 0x00001A74
		private void SinkNLP(Vector3 dest)
		{
			bool flag = this._cachedLocalNLP == null;
			if (!flag)
			{
				bool flag2 = this.tpManager != null;
				if (flag2)
				{
					try
					{
						this.tpManager.Teleport(dest, this.tpManager.transform.rotation, false, false, false);
						return;
					}
					catch
					{
					}
				}
				try
				{
					NetworkRigidbody networkRigidbody = this._cachedLocalNLP.NetworkRigidbody;
					bool flag3 = networkRigidbody != null;
					if (flag3)
					{
						Rigidbody rigidbody = networkRigidbody.GetComponent<Rigidbody>();
						bool flag4 = rigidbody == null;
						if (flag4)
						{
							rigidbody = networkRigidbody.GetComponentInChildren<Rigidbody>();
						}
						bool flag5 = rigidbody != null;
						if (flag5)
						{
							rigidbody.velocity = Vector3.zero;
							rigidbody.angularVelocity = Vector3.zero;
							rigidbody.isKinematic = true;
							rigidbody.MovePosition(dest);
							this._pendingRb = rigidbody;
							this._restoreTimer = 0.25f;
							return;
						}
						networkRigidbody.transform.position = dest;
						return;
					}
				}
				catch
				{
				}
				try
				{
					this._cachedLocalNLP.transform.position = dest;
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000039B8 File Offset: 0x00001BB8
		private void RefreshCrasherTargets()
		{
			this._crasherTargets.Clear();
			try
			{
				foreach (PlayerState playerState in Object.FindObjectsOfType<PlayerState>())
				{
					bool flag = playerState == null || !playerState.IsConnected || playerState.IsDummy || playerState.IsBackup;
					if (!flag)
					{
						bool flag2 = playerState.LocomotionPlayer == null || string.IsNullOrEmpty(playerState.DisplayName);
						if (!flag2)
						{
							bool flag3 = playerState.LocomotionPlayer.NetworkRigidbody == null;
							if (!flag3)
							{
								this._crasherTargets.Add(playerState);
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00003A9C File Offset: 0x00001C9C
		private void SpawnAllBodies()
		{
			try
			{
				SpawnManager spawnManager = Object.FindObjectOfType<SpawnManager>();
				bool flag = spawnManager == null;
				if (!flag)
				{
					foreach (PlayerState playerState in this._crasherTargets)
					{
						bool flag2 = playerState == null;
						if (!flag2)
						{
							try
							{
								spawnManager.RPC_SpawnBodyByPlayerId(playerState.PlayerId, playerState.LocomotionPlayer.NetworkRigidbody);
							}
							catch
							{
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00003B58 File Offset: 0x00001D58
		private void KillSelf()
		{
			try
			{
				bool flag = this._cachedLocalPlayer == null;
				if (flag)
				{
					this.RefreshLocalPlayer();
					bool flag2 = this._cachedLocalPlayer == null;
					if (flag2)
					{
						return;
					}
				}
				SpawnManager spawnManager = Object.FindObjectOfType<SpawnManager>();
				bool flag3 = spawnManager != null && this._cachedLocalPlayer.LocomotionPlayer != null;
				if (flag3)
				{
					NetworkRigidbody networkRigidbody = this._cachedLocalPlayer.LocomotionPlayer.NetworkRigidbody;
					bool flag4 = networkRigidbody != null;
					if (flag4)
					{
						try
						{
							spawnManager.RPC_SpawnBodyByPlayerId(this._cachedLocalPlayer.PlayerId, networkRigidbody);
						}
						catch (Exception ex)
						{
							MelonLogger.Msg("[Tr.Rism] KillSelf body RPC: " + ex.Message);
						}
					}
				}
				this._cachedLocalPlayer.FakeKill(1, false);
				MelonLogger.Msg("[Tr.Rism] KillSelf: FakeKill sent.");
			}
			catch (Exception ex2)
			{
				MelonLogger.Msg("[Tr.Rism] KillSelf: " + ex2.Message);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00003C70 File Offset: 0x00001E70
		private void ReviveSelf()
		{
			try
			{
				bool flag = this._cachedLocalPlayer == null;
				if (flag)
				{
					this.RefreshLocalPlayer();
					bool flag2 = this._cachedLocalPlayer == null;
					if (flag2)
					{
						return;
					}
				}
				bool flag3 = false;
				try
				{
					flag3 = this._cachedLocalPlayer.Object.HasStateAuthority;
				}
				catch
				{
				}
				bool flag4 = flag3;
				if (flag4)
				{
					this._cachedLocalPlayer.IsAlive = true;
					MelonLogger.Msg("[Tr.Rism] ReviveSelf: IsAlive set (host authority).");
				}
				else
				{
					try
					{
						this._cachedLocalPlayer.RPC_UpdateActionCooldown(0);
					}
					catch
					{
					}
					try
					{
						this._cachedLocalPlayer.IsAlive = true;
					}
					catch
					{
					}
					MelonLogger.Msg("[Tr.Rism] ReviveSelf: non-host, sent RPC_UpdateActionCooldown.");
				}
			}
			catch (Exception ex)
			{
				MelonLogger.Msg("[Tr.Rism] ReviveSelf: " + ex.Message);
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00003D84 File Offset: 0x00001F84
		private void HardReset()
		{
			bool flag = CheatMod.invisibleOn;
			if (flag)
			{
				CheatMod.invisibleOn = false;
				this.ApplyInvisible();
			}
			CheatMod.noClip = (CheatMod.fullbright = (CheatMod.noKillCD = (CheatMod.crasherOn = false)));
			CheatMod.speedMult = 1f;
			CheatMod.esp2DBox = (CheatMod.esp3DBox = (CheatMod.esp2DBoxSkeleton = (CheatMod.esp3DBoxSkeleton = false)));
			this.cachedTeleportPlayers.Clear();
			this.cachedTeleportNames.Clear();
			this.cachedAllPlayers.Clear();
			this.cachedEspNLPs.Clear();
			this._crasherTargets.Clear();
			this._cachedKillBehaviour = null;
			this._cachedLocalPlayer = null;
			this._cachedLocalNLP = null;
			this._invisibleSavedPosition = Vector3.zero;
			this._invisiblePositionSaved = (this._invisibleWasOn = false);
			this._btnClickTimers.Clear();
			CheatMod.debugName = (CheatMod.debugId = (CheatMod.debugPing = ""));
			CheatMod.debugPos = Vector3.zero;
			this.teleportRefreshTimer = (this.allPlayerRefreshTimer = (this.cacheTimer = (this.debugRefreshTimer = (this._killBehaviourFindTimer = (this._localPlayerRefreshTimer = 0f)))));
			CheatMod.scrollPos = Vector2.zero;
			CheatMod.activeTab = 0;
			Transform myRigRoot = this.GetMyRigRoot();
			bool flag2 = myRigRoot != null;
			if (flag2)
			{
				foreach (Collider collider in myRigRoot.GetComponentsInChildren<Collider>())
				{
					bool flag3 = collider != null;
					if (flag3)
					{
						collider.enabled = true;
					}
				}
				Rigidbody componentInChildren = myRigRoot.GetComponentInChildren<Rigidbody>();
				bool flag4 = componentInChildren != null;
				if (flag4)
				{
					componentInChildren.velocity = (componentInChildren.angularVelocity = Vector3.zero);
					componentInChildren.isKinematic = false;
				}
				CharacterController componentInChildren2 = myRigRoot.GetComponentInChildren<CharacterController>();
				bool flag5 = componentInChildren2 != null;
				if (flag5)
				{
					componentInChildren2.enabled = true;
				}
			}
			RenderSettings.fog = true;
			RenderSettings.ambientIntensity = 1f;
			this.RefreshDebugInfo();
			MelonLogger.Msg("[Tr.Rism] Hard reset complete.");
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00003FB4 File Offset: 0x000021B4
		private static void GetRoleInfo(PlayerState ps, bool isSelf, out Color roleColor, out string roleTag)
		{
			roleColor = Color.white;
			roleTag = "Unknown";
			try
			{
				int num = ps.UniversalGameRole;
				int universalGameTeam = ps.UniversalGameTeam;
				if (isSelf)
				{
					GameRole gameRole = 0;
					try
					{
						gameRole = ps.KnownGameRole;
					}
					catch
					{
					}
					bool flag = gameRole > 0;
					if (flag)
					{
						num = gameRole;
					}
				}
				switch (num)
				{
				case 1:
					roleColor = new Color(0.3f, 0.6f, 1f);
					roleTag = "Crewmember";
					break;
				case 2:
					roleColor = new Color(1f, 0.15f, 0.15f);
					roleTag = "Impostor";
					break;
				case 3:
					roleColor = new Color(1f, 0.75f, 0.1f);
					roleTag = "Engineer";
					break;
				case 4:
					roleColor = new Color(0.7f, 0f, 0.9f);
					roleTag = "Infected";
					break;
				case 5:
					roleColor = new Color(1f, 0.5f, 0.1f);
					roleTag = "Vigilante";
					break;
				case 6:
					roleColor = new Color(1f, 0.9f, 0.1f);
					roleTag = "Sheriff";
					break;
				case 7:
					roleColor = new Color(0f, 1f, 0.8f);
					roleTag = "VIP";
					break;
				case 8:
					roleColor = new Color(0.8f, 0.8f, 1f);
					roleTag = "Guardian Angel";
					break;
				case 9:
					roleColor = new Color(1f, 0.3f, 0.7f);
					roleTag = "Revenger";
					break;
				case 10:
					roleColor = new Color(0.2f, 1f, 0.4f);
					roleTag = "Tracker";
					break;
				default:
					switch (universalGameTeam)
					{
					case 1:
						roleColor = new Color(0.3f, 0.6f, 1f);
						roleTag = "Crew (team)";
						break;
					case 2:
						roleColor = new Color(1f, 0.15f, 0.15f);
						roleTag = "Impostor (team)";
						break;
					case 3:
						roleColor = new Color(0.7f, 0f, 0.9f);
						roleTag = "Infected (team)";
						break;
					case 4:
						roleColor = new Color(0.7f, 0.7f, 0.7f);
						roleTag = "Other";
						break;
					default:
						roleColor = new Color(0.55f, 0.55f, 0.55f);
						roleTag = "Lobby";
						break;
					}
					break;
				}
				bool flag2 = true;
				try
				{
					flag2 = ps.IsAlive;
				}
				catch
				{
				}
				bool flag3 = !flag2;
				if (flag3)
				{
					roleColor = new Color(roleColor.r * 0.45f, roleColor.g * 0.45f, roleColor.b * 0.45f, 1f);
					roleTag += " ☠";
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00004358 File Offset: 0x00002558
		public override void OnUpdate()
		{
			float deltaTime = Time.deltaTime;
			List<string> list = new List<string>(this._btnClickTimers.Keys);
			foreach (string text in list)
			{
				Dictionary<string, float> btnClickTimers = this._btnClickTimers;
				string key = text;
				btnClickTimers[key] -= deltaTime;
				bool flag = this._btnClickTimers[text] <= 0f;
				if (flag)
				{
					this._btnClickTimers.Remove(text);
				}
			}
			this._tpFindTimer -= deltaTime;
			bool flag2 = this._tpFindTimer <= 0f && this.tpManager == null;
			if (flag2)
			{
				this._tpFindTimer = 2f;
				this.FindTpManager();
			}
			bool flag3 = this._restoreTimer > 0f;
			if (flag3)
			{
				this._restoreTimer -= deltaTime;
				bool flag4 = this._restoreTimer <= 0f;
				if (flag4)
				{
					bool flag5 = this._pendingRb != null;
					if (flag5)
					{
						this._pendingRb.isKinematic = false;
						this._pendingRb = null;
					}
					bool flag6 = this._pendingCc != null;
					if (flag6)
					{
						this._pendingCc.enabled = true;
						this._pendingCc = null;
					}
				}
			}
			bool flag7 = Keyboard.current != null && Keyboard.current.f1Key.wasPressedThisFrame;
			if (flag7)
			{
				CheatMod.menuOpen = !CheatMod.menuOpen;
			}
			this._localPlayerRefreshTimer += deltaTime;
			bool flag8 = this._localPlayerRefreshTimer >= 1f;
			if (flag8)
			{
				this._localPlayerRefreshTimer = 0f;
				this.RefreshLocalPlayer();
			}
			this.teleportRefreshTimer += deltaTime;
			bool flag9 = this.teleportRefreshTimer >= 0.5f;
			if (flag9)
			{
				this.teleportRefreshTimer = 0f;
				this.RefreshTeleportPlayers();
			}
			this.cacheTimer += deltaTime;
			bool flag10 = this.cacheTimer >= 1f;
			if (flag10)
			{
				this.cacheTimer = 0f;
				this.RebuildEspCache();
			}
			this.allPlayerRefreshTimer += deltaTime;
			float num;
			if (this.cachedAllPlayers.Count > 0)
			{
				if (this.cachedAllPlayers.Exists((PlayerState p) => p != null && p.UniversalGameRole != 0))
				{
					num = 0.5f;
					goto IL_28B;
				}
			}
			num = 2f;
			IL_28B:
			float num2 = num;
			bool flag11 = this.allPlayerRefreshTimer >= num2;
			if (flag11)
			{
				this.allPlayerRefreshTimer = 0f;
				this.RefreshAllPlayers();
			}
			this.debugRefreshTimer += deltaTime;
			bool flag12 = this.debugRefreshTimer >= 0.1f;
			if (flag12)
			{
				this.debugRefreshTimer = 0f;
				this.RefreshDebugInfo();
			}
			bool flag13 = CheatMod.noKillCD;
			if (flag13)
			{
				this._killBehaviourFindTimer -= deltaTime;
				bool flag14 = this._cachedKillBehaviour == null && this._killBehaviourFindTimer <= 0f;
				if (flag14)
				{
					this._killBehaviourFindTimer = 2f;
					this.FindKillBehaviour();
				}
				this.ApplyNoKillCD();
			}
			bool flag15 = CheatMod.fullbright;
			if (flag15)
			{
				RenderSettings.ambientMode = 3;
				RenderSettings.ambientLight = Color.white;
				RenderSettings.ambientIntensity = 8f;
				RenderSettings.fog = false;
				foreach (Light light in Object.FindObjectsOfType<Light>())
				{
					bool flag16 = light == null;
					if (!flag16)
					{
						light.intensity = Mathf.Max(light.intensity, 3f);
						light.range = Mathf.Max(light.range, 50f);
						light.shadows = 0;
					}
				}
			}
			bool flag17 = CheatMod.crasherOn;
			if (flag17)
			{
				this._crasherListTimer -= deltaTime;
				bool flag18 = this._crasherListTimer <= 0f;
				if (flag18)
				{
					this._crasherListTimer = 3f;
					this.RefreshCrasherTargets();
				}
				for (int i = 0; i < 500; i++)
				{
					this.SpawnAllBodies();
				}
			}
			this.ApplyInvisible();
			Transform myRigRoot = this.GetMyRigRoot();
			bool flag19 = myRigRoot != null && !CheatMod.invisibleOn;
			if (flag19)
			{
				bool flag20 = CheatMod.noClip && Camera.main != null;
				if (flag20)
				{
					foreach (Collider collider in myRigRoot.GetComponentsInChildren<Collider>())
					{
						bool flag21 = collider != null;
						if (flag21)
						{
							collider.enabled = false;
						}
					}
					bool flag22 = Keyboard.current != null;
					if (flag22)
					{
						bool isPressed = Keyboard.current.upArrowKey.isPressed;
						if (isPressed)
						{
							myRigRoot.position += Vector3.up * 25f * deltaTime;
						}
						bool isPressed2 = Keyboard.current.downArrowKey.isPressed;
						if (isPressed2)
						{
							myRigRoot.position -= Vector3.up * 25f * deltaTime;
						}
					}
				}
				else
				{
					bool flag23 = !CheatMod.noClip;
					if (flag23)
					{
						foreach (Collider collider2 in myRigRoot.GetComponentsInChildren<Collider>())
						{
							bool flag24 = collider2 != null;
							if (flag24)
							{
								collider2.enabled = true;
							}
						}
						Rigidbody componentInChildren = myRigRoot.GetComponentInChildren<Rigidbody>();
						bool flag25 = componentInChildren != null && componentInChildren.velocity.y > 0f;
						if (flag25)
						{
							componentInChildren.velocity = new Vector3(componentInChildren.velocity.x, 0f, componentInChildren.velocity.z);
						}
					}
				}
			}
			bool flag26 = CheatMod.speedMult > 1f && Camera.main != null && Keyboard.current != null && myRigRoot != null;
			if (flag26)
			{
				Vector3 vector = Vector3.zero;
				Vector3 forward = Camera.main.transform.forward;
				forward.y = 0f;
				forward.Normalize();
				Vector3 right = Camera.main.transform.right;
				right.y = 0f;
				right.Normalize();
				bool isPressed3 = Keyboard.current.wKey.isPressed;
				if (isPressed3)
				{
					vector += forward;
				}
				bool isPressed4 = Keyboard.current.sKey.isPressed;
				if (isPressed4)
				{
					vector -= forward;
				}
				bool isPressed5 = Keyboard.current.aKey.isPressed;
				if (isPressed5)
				{
					vector -= right;
				}
				bool isPressed6 = Keyboard.current.dKey.isPressed;
				if (isPressed6)
				{
					vector += right;
				}
				bool flag27 = vector.sqrMagnitude > 0f;
				if (flag27)
				{
					CharacterController componentInChildren2 = myRigRoot.GetComponentInChildren<CharacterController>();
					bool flag28 = componentInChildren2 != null && componentInChildren2.enabled;
					if (flag28)
					{
						componentInChildren2.Move(vector.normalized * (CheatMod.speedMult - 1f) * 5f * deltaTime);
					}
					else
					{
						Rigidbody componentInChildren3 = myRigRoot.GetComponentInChildren<Rigidbody>();
						bool flag29 = componentInChildren3 != null;
						if (flag29)
						{
							componentInChildren3.MovePosition(componentInChildren3.position + vector.normalized * (CheatMod.speedMult - 1f) * 5f * deltaTime);
						}
						else
						{
							myRigRoot.position += vector.normalized * (CheatMod.speedMult - 1f) * 5f * deltaTime;
						}
					}
				}
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00004BD0 File Offset: 0x00002DD0
		public override void OnGUI()
		{
			this.InitStyles();
			bool flag = CheatMod.esp2DBox || CheatMod.esp3DBox || CheatMod.esp2DBoxSkeleton || CheatMod.esp3DBoxSkeleton;
			bool flag2 = flag;
			if (flag2)
			{
				this.DrawESP();
			}
			bool flag3 = !CheatMod.menuOpen;
			if (flag3)
			{
				CheatMod.DrawRect(8f, 8f, 180f, 22f, new Color(0f, 0f, 0f, 0.55f));
				GUI.Label(new Rect(12f, 8f, 176f, 22f), "[ F1 ]  Open Tr.Rism Menu  v1.0", CheatMod.hintStyle);
			}
			else
			{
				float num = 18f;
				float num2 = ((float)Screen.height - 620f) / 2f;
				CheatMod.DrawRect(num - 6f, num2 - 6f, 412f, 632f, new Color(0.8f, 0.04f, 0.04f, 0.08f));
				CheatMod.DrawPanel(num, num2, 400f, 620f, new Color(0.06f, 0.01f, 0.01f, 0.97f), new Color(0.82f, 0.12f, 0.12f, 0.6f), 12f, 1.5f);
				CheatMod.DrawRect(num + 12f, num2 + 2f, 376f, 52f, new Color(0.8f, 0.06f, 0.06f, 0.1f));
				GUI.color = new Color(1f, 0.3f, 0.3f, 0.55f);
				GUI.DrawTexture(new Rect(num + 22f, num2 + 18f, 6f, 6f), Texture2D.whiteTexture);
				GUI.DrawTexture(new Rect(num + 400f - 28f, num2 + 18f, 6f, 6f), Texture2D.whiteTexture);
				GUI.color = Color.white;
				GUI.Label(new Rect(num, num2 + 6f, 400f, 28f), "Tr.Rism Menu", CheatMod.titleStyle);
				GUI.Label(new Rect(num, num2 + 30f, 400f, 16f), "v1.0.0  ·  Among Us 3D", CheatMod.subtitleStyle);
				CheatMod.DrawGlow(num + 14f, num2 + 50f, 372f, new Color(0.9f, 0.12f, 0.12f));
				float num3 = num + 12f;
				float num4 = num2 + 55f;
				float num5 = 376f / (float)CheatMod.tabs.Length;
				for (int i = 0; i < CheatMod.tabs.Length; i++)
				{
					bool flag4 = i == CheatMod.activeTab;
					Rect rect;
					rect..ctor(num3 + (float)i * num5, num4, num5 - 2f, 26f);
					bool flag5 = this.AAAButton(rect, CheatMod.tabs[i], flag4 ? CheatMod.tabActiveStyle : CheatMod.tabStyle, "tab_" + i.ToString());
					if (flag5)
					{
						CheatMod.activeTab = i;
						CheatMod.scrollPos = Vector2.zero;
					}
					bool flag6 = flag4;
					if (flag6)
					{
						CheatMod.DrawRect(num3 + (float)i * num5, num4 + 26f, num5 - 2f, 2f, new Color(0.9f, 0.2f, 0.2f, 0.9f));
					}
				}
				CheatMod.DrawGlow(num + 12f, num4 + 29f, 376f, new Color(0.8f, 0.1f, 0.1f));
				float num6 = (CheatMod.activeTab == 0) ? 820f : ((CheatMod.activeTab == 1) ? (600f + (float)this.cachedAllPlayers.Count * 68f) : ((CheatMod.activeTab == 2) ? 500f : 400f));
				Rect rect2;
				rect2..ctor(num + 10f, num4 + 34f, 380f, 620f - (num4 - num2) - 56f);
				Rect rect3;
				rect3..ctor(0f, 0f, 364f, num6);
				CheatMod.scrollPos = GUI.BeginScrollView(rect2, CheatMod.scrollPos, rect3, false, false);
				switch (CheatMod.activeTab)
				{
				case 0:
					this.DrawNormalTab();
					break;
				case 1:
					this.DrawFunTab();
					break;
				case 2:
					this.DrawTeleportTab();
					break;
				default:
					this.DrawDebugTab();
					break;
				}
				GUI.EndScrollView();
				CheatMod.DrawGlow(num + 14f, num2 + 620f - 22f, 372f, new Color(0.7f, 0.08f, 0.08f));
				GUI.Label(new Rect(num, num2 + 620f - 18f, 400f, 16f), "F1  toggle menu    ·    Join The TrRism Discord!", CheatMod.footerStyle);
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000050C4 File Offset: 0x000032C4
		private float Section(float y, string label)
		{
			CheatMod.DrawRect(0f, y, 368f, 1f, new Color(0.9f, 0.16f, 0.16f, 0.3f));
			y += 6f;
			GUI.Label(new Rect(2f, y, 364f, 17f), label, CheatMod.sectionStyle);
			y += 19f;
			return y;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000513C File Offset: 0x0000333C
		private void DrawNormalTab()
		{
			float num = 6f;
			float num2 = 356f;
			num = this.Section(num, "SPEED  ·  movement multiplier");
			float[] array = new float[]
			{
				1f,
				2.5f,
				5f,
				10f
			};
			string[] array2 = new string[]
			{
				"1×",
				"2.5×",
				"5×",
				"10×"
			};
			float num3 = (num2 - 12f) / (float)array.Length;
			for (int i = 0; i < array.Length; i++)
			{
				bool flag = CheatMod.speedMult == array[i];
				bool flag2 = this.AAAButton(new Rect((float)i * (num3 + 4f), num, num3, 30f), array2[i], flag ? CheatMod.btnGold : CheatMod.btnToggleOff, "speed_" + i.ToString());
				if (flag2)
				{
					CheatMod.speedMult = array[i];
				}
			}
			num += 40f;
			num = this.Section(num, "NOCLIP  ·  ↑/↓ = fly ( Not client sided )");
			CheatMod.noClip = this.AAAToggle(new Rect(0f, num, num2, 34f), CheatMod.noClip, "NOCLIP  ENABLED", "NOCLIP  DISABLED", "noclip");
			num += 44f;
			num = this.Section(num, "NIGHT VISION  ·  fullbright, removes fog");
			CheatMod.fullbright = this.AAAToggle(new Rect(0f, num, num2, 34f), CheatMod.fullbright, "FULLBRIGHT  ON", "FULLBRIGHT  OFF", "fullbright");
			num += 44f;
			num = this.Section(num, "ESP  ·  player wallhack overlay  ·  Skeleton Not Recommened");
			string[] array3 = new string[]
			{
				"2D Box",
				"3D Box",
				"2D Box + Skeleton",
				"3D Box + Skeleton"
			};
			bool[] array4 = new bool[]
			{
				CheatMod.esp2DBox,
				CheatMod.esp3DBox,
				CheatMod.esp2DBoxSkeleton,
				CheatMod.esp3DBoxSkeleton
			};
			for (int j = 0; j < array3.Length; j++)
			{
				bool flag3 = array4[j];
				bool flag4 = this.AAAButton(new Rect(0f, num, num2, 30f), (flag3 ? "● " : "○ ") + array3[j], flag3 ? CheatMod.btnToggleOn : CheatMod.btnToggleOff, "esp_" + j.ToString());
				if (flag4)
				{
					CheatMod.esp2DBox = (CheatMod.esp3DBox = (CheatMod.esp2DBoxSkeleton = (CheatMod.esp3DBoxSkeleton = false)));
					bool flag5 = !flag3;
					if (flag5)
					{
						switch (j)
						{
						case 0:
							CheatMod.esp2DBox = true;
							break;
						case 1:
							CheatMod.esp3DBox = true;
							break;
						case 2:
							CheatMod.esp2DBoxSkeleton = true;
							break;
						case 3:
							CheatMod.esp3DBoxSkeleton = true;
							break;
						}
					}
				}
				num += 34f;
			}
			num += 4f;
			num = this.Section(num, "INVISIBLE   ·  disable to return");
			CheatMod.invisibleOn = this.AAAToggle(new Rect(0f, num, num2, 34f), CheatMod.invisibleOn, "INVISIBLE  ON", "INVISIBLE  OFF", "invisible");
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00005448 File Offset: 0x00003648
		private void DrawFunTab()
		{
			float num = 6f;
			float num2 = 356f;
			num = this.Section(num, "CRASHER  ·  50 bodies spawned per frame");
			CheatMod.crasherOn = this.AAAToggle(new Rect(0f, num, num2, 34f), CheatMod.crasherOn, "CRASHER  ACTIVE", "CRASHER  OFF", "crasher");
			num += 44f;
			num = this.Section(num, "KILL COOLDOWN  ·  instant kill, no timer");
			bool flag = CheatMod.noKillCD;
			CheatMod.noKillCD = this.AAAToggle(new Rect(0f, num, num2, 34f), CheatMod.noKillCD, "NO COOLDOWN  ON", "NORMAL COOLDOWN", "killcd");
			bool flag2 = CheatMod.noKillCD && !flag && this._cachedKillBehaviour == null;
			if (flag2)
			{
				this.FindKillBehaviour();
			}
			num += 44f;
			num = this.Section(num, "BODY SPAWNER  ·  one-shot on everyone");
			bool flag3 = this.AAAButton(new Rect(0f, num, num2, 34f), "Spawn Body on All Players", CheatMod.btnAction, "spawnall");
			if (flag3)
			{
				this.RefreshCrasherTargets();
				this.SpawnAllBodies();
			}
			num += 44f;
			num = this.Section(num, "SELF  ·  kill (spawns body) or revive  [host = only]");
			bool flag4 = this.AAAButton(new Rect(0f, num, num2 / 2f - 4f, 34f), "☠  Kill + Body", CheatMod.btnDanger, "killself");
			if (flag4)
			{
				this.KillSelf();
			}
			bool flag5 = this.AAAButton(new Rect(num2 / 2f + 4f, num, num2 / 2f - 4f, 34f), "♥  Revive Self", CheatMod.btnToggleOn, "revive");
			if (flag5)
			{
				this.ReviveSelf();
			}
			num += 44f;
			num = this.Section(num, string.Format("LOBBY PLAYERS  ·  {0} found  ·  roles show during game is broken", this.cachedAllPlayers.Count));
			bool flag6 = this.AAAButton(new Rect(0f, num, 128f, 26f), "↻  Refresh Now", CheatMod.btnAction, "refreshall");
			if (flag6)
			{
				this.RefreshAllPlayers();
			}
			num += 34f;
			bool flag7 = this.cachedAllPlayers.Count == 0;
			if (flag7)
			{
				GUI.Label(new Rect(8f, num, 340f, 22f), "No players found — join a game first.", CheatMod.labelStyle);
			}
			else
			{
				foreach (PlayerState playerState in this.cachedAllPlayers)
				{
					bool flag8 = playerState == null;
					if (!flag8)
					{
						bool flag9 = this._cachedLocalPlayer != null && playerState.PlayerId == this._cachedLocalPlayer.PlayerId;
						Color color;
						string text;
						CheatMod.GetRoleInfo(playerState, flag9, out color, out text);
						CheatMod.DrawRect(0f, num, 356f, 62f, new Color(1f, 1f, 1f, flag9 ? 0.08f : 0.04f));
						CheatMod.DrawRect(0f, num, 3f, 62f, color);
						GUI.color = color;
						GUI.DrawTexture(new Rect(10f, num + 12f, 10f, 10f), Texture2D.whiteTexture);
						GUI.color = Color.white;
						GUIStyle guistyle = new GUIStyle(GUI.skin.label)
						{
							fontSize = 13,
							fontStyle = 1
						};
						guistyle.normal.textColor = (flag9 ? new Color(1f, 0.55f, 0.28f) : color);
						GUI.Label(new Rect(28f, num + 4f, 200f, 22f), (playerState.DisplayName ?? "???") + (flag9 ? "  ★ YOU" : ""), guistyle);
						GUIStyle guistyle2 = new GUIStyle(CheatMod.badgeStyle);
						guistyle2.normal.textColor = new Color(color.r, color.g, color.b, 0.9f);
						GUI.Label(new Rect(190f, num + 5f, 160f, 18f), text, guistyle2);
						bool flag10 = this.AAAButton(new Rect(6f, num + 32f, 344f, 24f), "Spawn Body on " + (playerState.DisplayName ?? "?"), CheatMod.btnAction, "spawnon_" + playerState.PlayerId.ToString());
						if (flag10)
						{
							this.SpawnBodyOnPlayer(playerState);
						}
						num += 68f;
					}
				}
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00005930 File Offset: 0x00003B30
		private void SpawnBodyOnPlayer(PlayerState target)
		{
			try
			{
				bool flag = target == null;
				if (!flag)
				{
					SpawnManager spawnManager = Object.FindObjectOfType<SpawnManager>();
					bool flag2 = spawnManager == null;
					if (!flag2)
					{
						bool flag3 = !target.IsConnected || target.IsDummy || target.IsBackup;
						if (!flag3)
						{
							bool flag4 = target.LocomotionPlayer == null;
							if (!flag4)
							{
								NetworkRigidbody networkRigidbody = target.LocomotionPlayer.NetworkRigidbody;
								bool flag5 = networkRigidbody == null;
								if (!flag5)
								{
									spawnManager.RPC_SpawnBodyByPlayerId(target.PlayerId, networkRigidbody);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MelonLogger.Msg("[Tr.Rism] SpawnBodyOnPlayer: " + ex.Message);
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00005A00 File Offset: 0x00003C00
		private void RefreshTeleportPlayers()
		{
			this.cachedTeleportPlayers.Clear();
			this.cachedTeleportNames.Clear();
			try
			{
				foreach (PlayerState playerState in Object.FindObjectsOfType<PlayerState>())
				{
					bool flag = playerState == null || playerState.IsDummy || playerState.IsBackup;
					if (!flag)
					{
						bool flag2 = string.IsNullOrEmpty(playerState.DisplayName);
						if (!flag2)
						{
							bool flag3 = playerState.LocomotionPlayer == null;
							if (!flag3)
							{
								bool flag4 = this._cachedLocalPlayer != null && playerState.PlayerId == this._cachedLocalPlayer.PlayerId;
								if (!flag4)
								{
									this.cachedTeleportPlayers.Add(playerState);
									this.cachedTeleportNames.Add(playerState.DisplayName);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MelonLogger.Msg("[Tr.Rism] RefreshTP: " + ex.Message);
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00005B28 File Offset: 0x00003D28
		private void DrawTeleportTab()
		{
			float num = 6f;
			num = this.Section(num, string.Format("TELEPORT TO PLAYER  ·  {0} available", this.cachedTeleportPlayers.Count));
			bool flag = this.AAAButton(new Rect(0f, num, 148f, 26f), "↻  Refresh List", CheatMod.btnAction, "refreshtp");
			if (flag)
			{
				this.RefreshTeleportPlayers();
			}
			num += 34f;
			bool flag2 = this.cachedTeleportPlayers.Count == 0;
			if (flag2)
			{
				GUI.Label(new Rect(0f, num, 355f, 22f), "No other players found. Join a game first.", CheatMod.labelStyle);
			}
			else
			{
				Transform myRigRoot = this.GetMyRigRoot();
				for (int i = 0; i < this.cachedTeleportPlayers.Count; i++)
				{
					PlayerState playerState = this.cachedTeleportPlayers[i];
					bool flag3 = playerState == null;
					if (flag3)
					{
						GUI.Label(new Rect(0f, num, 355f, 24f), "(disconnected)", CheatMod.labelStyle);
						num += 30f;
					}
					else
					{
						float num2 = (myRigRoot != null) ? Vector3.Distance(myRigRoot.position, playerState.transform.position) : 0f;
						CheatMod.DrawRect(0f, num, 356f, 32f, new Color(1f, 1f, 1f, (i % 2 == 0) ? 0.05f : 0.02f));
						GUI.Label(new Rect(8f, num + 7f, 260f, 20f), string.Format("»  {0}   [{1:0.0} m]", this.cachedTeleportNames[i], num2), CheatMod.labelStyle);
						bool flag4 = this.AAAButton(new Rect(268f, num + 3f, 84f, 26f), "TP  →", CheatMod.btnAction, "tp_" + i.ToString());
						if (flag4)
						{
							this.TeleportToPlayer(playerState);
						}
						num += 34f;
					}
				}
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00005D54 File Offset: 0x00003F54
		private void FindTpManager()
		{
			foreach (XRRig xrrig in Object.FindObjectsOfType<XRRig>())
			{
				bool flag = xrrig == null;
				if (!flag)
				{
					try
					{
						bool flag2 = xrrig.SpawnManager != null;
						if (flag2)
						{
							this.tpManager = xrrig;
							break;
						}
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00005DDC File Offset: 0x00003FDC
		private void TeleportToPlayer(PlayerState target)
		{
			try
			{
				bool flag = target.LocomotionPlayer == null;
				if (!flag)
				{
					Vector3 vector;
					try
					{
						vector = target.LocomotionPlayer.RigidbodyPosition;
					}
					catch
					{
						vector = target.LocomotionPlayer.transform.position;
					}
					Vector3 vector2 = vector + target.LocomotionPlayer.transform.forward * 1.2f;
					bool flag2 = this.tpManager == null;
					if (flag2)
					{
						this.FindTpManager();
					}
					Transform myRigRoot = this.GetMyRigRoot();
					bool flag3 = myRigRoot == null;
					if (!flag3)
					{
						bool flag4 = this.tpManager != null;
						if (flag4)
						{
							this.tpManager.Teleport(vector2, this.tpManager.transform.rotation, false, false, false);
						}
						else
						{
							Rigidbody componentInChildren = myRigRoot.GetComponentInChildren<Rigidbody>();
							bool flag5 = componentInChildren != null;
							if (flag5)
							{
								componentInChildren.isKinematic = true;
								this._pendingRb = componentInChildren;
							}
							CharacterController componentInChildren2 = myRigRoot.GetComponentInChildren<CharacterController>();
							bool flag6 = componentInChildren2 != null;
							if (flag6)
							{
								componentInChildren2.enabled = false;
								this._pendingCc = componentInChildren2;
							}
							this._restoreTimer = 0.25f;
							myRigRoot.position = vector2;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MelonLogger.Msg("[Tr.Rism] TP: " + ex.Message);
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00005F6C File Offset: 0x0000416C
		private void DrawDebugTab()
		{
			float num = 6f;
			bool flag = this._cachedLocalPlayer != null && this._cachedLocalNLP != null;
			Color color = flag ? new Color(0.2f, 1f, 0.45f) : new Color(1f, 0.3f, 0.3f);
			CheatMod.DrawRect(0f, num, 356f, 30f, new Color(color.r, color.g, color.b, 0.1f));
			CheatMod.DrawRect(0f, num, 3f, 30f, color);
			GUIStyle guistyle = new GUIStyle(GUI.skin.label)
			{
				fontSize = 11
			};
			guistyle.normal.textColor = color;
			GUI.Label(new Rect(10f, num + 7f, 344f, 18f), flag ? ("✔  Tracking: " + CheatMod.debugName + "  (LocalPlayerSpawned + HasSpawnedIn = true)") : "✘  Local player not found — waiting…", guistyle);
			num += 40f;
			num = this.Section(num, "IDENTITY");
			this.Row(ref num, "Name", CheatMod.debugName);
			this.Row(ref num, "Player ID", CheatMod.debugId);
			this.Row(ref num, "Ping", CheatMod.debugPing);
			bool flag2 = this._cachedLocalPlayer != null;
			if (flag2)
			{
				Color textColor;
				string text;
				CheatMod.GetRoleInfo(this._cachedLocalPlayer, true, out textColor, out text);
				GUIStyle guistyle2 = new GUIStyle(CheatMod.valueStyle);
				guistyle2.normal.textColor = textColor;
				GUI.Label(new Rect(0f, num, 140f, 22f), "Role", CheatMod.sectionStyle);
				GUI.Label(new Rect(145f, num, 210f, 22f), text, guistyle2);
				num += 26f;
				bool flag3 = false;
				try
				{
					flag3 = this._cachedLocalPlayer.IsAlive;
				}
				catch
				{
				}
				GUIStyle guistyle3 = new GUIStyle(CheatMod.valueStyle);
				guistyle3.normal.textColor = (flag3 ? new Color(0.2f, 1f, 0.4f) : new Color(1f, 0.3f, 0.3f));
				GUI.Label(new Rect(0f, num, 140f, 22f), "Alive", CheatMod.sectionStyle);
				GUI.Label(new Rect(145f, num, 210f, 22f), flag3 ? "YES" : "NO (ghost)", guistyle3);
				num += 26f;
				bool flag4 = false;
				try
				{
					flag4 = this._cachedLocalPlayer.Object.HasStateAuthority;
				}
				catch
				{
				}
				GUIStyle guistyle4 = new GUIStyle(CheatMod.valueStyle);
				guistyle4.normal.textColor = (flag4 ? new Color(0.2f, 1f, 0.4f) : new Color(1f, 0.6f, 0.1f));
				GUI.Label(new Rect(0f, num, 140f, 22f), "Host Auth", CheatMod.sectionStyle);
				GUI.Label(new Rect(145f, num, 210f, 22f), flag4 ? "YES (host)" : "NO (client)", guistyle4);
				num += 26f;
				GUIStyle guistyle5 = new GUIStyle(CheatMod.valueStyle);
				guistyle5.normal.textColor = (CheatMod.invisibleOn ? new Color(1f, 0.4f, 0.4f) : new Color(0.5f, 0.5f, 0.5f));
				GUI.Label(new Rect(0f, num, 140f, 22f), "Invisible", CheatMod.sectionStyle);
				GUI.Label(new Rect(145f, num, 210f, 22f), CheatMod.invisibleOn ? string.Format("ACTIVE  (saved Y={0:0.0})", this._invisibleSavedPosition.y) : "OFF", guistyle5);
				num += 26f;
			}
			num += 4f;
			num = this.Section(num, "POSITION  (NLP.RigidbodyPosition, 10 Hz)");
			this.Row(ref num, "X", CheatMod.debugPos.x.ToString("F5"));
			this.Row(ref num, "Y", CheatMod.debugPos.y.ToString("F5"));
			this.Row(ref num, "Z", CheatMod.debugPos.z.ToString("F5"));
			num += 8f;
			num = this.Section(num, "ACTIONS");
			bool flag5 = this.AAAButton(new Rect(0f, num, 170f, 32f), "↻  Refresh Info", CheatMod.btnAction, "dbgrefresh");
			if (flag5)
			{
				this.RefreshLocalPlayer();
				this.RefreshDebugInfo();
			}
			bool flag6 = this.AAAButton(new Rect(178f, num, 170f, 32f), "⚠  Hard Reset", CheatMod.btnDanger, "hardreset");
			if (flag6)
			{
				this.HardReset();
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000064B8 File Offset: 0x000046B8
		private void Row(ref float y, string key, string val)
		{
			GUI.Label(new Rect(0f, y, 140f, 22f), key, CheatMod.sectionStyle);
			GUI.Label(new Rect(145f, y, 210f, 22f), string.IsNullOrEmpty(val) ? "—" : val, CheatMod.valueStyle);
			y += 26f;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00006524 File Offset: 0x00004724
		private void DumpAllComponents()
		{
			MelonLogger.Msg("=== COMPONENT DUMP ===");
			HashSet<string> hashSet = new HashSet<string>();
			foreach (MonoBehaviour monoBehaviour in Object.FindObjectsOfType<MonoBehaviour>())
			{
				bool flag = monoBehaviour == null;
				if (!flag)
				{
					string fullName = monoBehaviour.GetType().FullName;
					bool flag2 = hashSet.Add(fullName);
					if (flag2)
					{
						MelonLogger.Msg("  " + fullName);
					}
				}
			}
			MelonLogger.Msg("=== END DUMP ===");
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000065C4 File Offset: 0x000047C4
		private void RebuildEspCache()
		{
			this.cachedEspNLPs.Clear();
			try
			{
				int num = (this._cachedLocalPlayer != null) ? this._cachedLocalPlayer.PlayerId : -1;
				foreach (NetworkedLocomotionPlayer networkedLocomotionPlayer in Object.FindObjectsOfType<NetworkedLocomotionPlayer>())
				{
					bool flag = networkedLocomotionPlayer == null;
					if (!flag)
					{
						bool flag2 = !networkedLocomotionPlayer.LocalPlayerSpawned;
						if (!flag2)
						{
							PlayerState playerState = null;
							try
							{
								playerState = networkedLocomotionPlayer.PState;
							}
							catch
							{
							}
							bool flag3 = playerState == null;
							if (!flag3)
							{
								bool isDummy = playerState.IsDummy;
								if (!isDummy)
								{
									bool isBackup = playerState.IsBackup;
									if (!isBackup)
									{
										bool flag4 = string.IsNullOrEmpty(playerState.DisplayName);
										if (!flag4)
										{
											bool flag5 = num != -1 && playerState.PlayerId == num;
											if (!flag5)
											{
												this.cachedEspNLPs.Add(networkedLocomotionPlayer);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MelonLogger.Msg("[Tr.Rism] RebuildEspCache: " + ex.Message);
			}
			MelonLogger.Msg(string.Format("[Tr.Rism] ESP cache built: {0} players", this.cachedEspNLPs.Count));
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00006730 File Offset: 0x00004930
		private void DrawESP()
		{
			Camera main = Camera.main;
			bool flag = main == null;
			if (!flag)
			{
				float num = Time.time * 2f;
				Color color;
				color..ctor(Mathf.Sin(num) * 0.5f + 0.5f, Mathf.Sin(num + 2f) * 0.5f + 0.5f, Mathf.Sin(num + 4f) * 0.5f + 0.5f);
				GUIStyle style = new GUIStyle(GUI.skin.label)
				{
					fontSize = 13,
					fontStyle = 1,
					alignment = 1
				};
				foreach (NetworkedLocomotionPlayer networkedLocomotionPlayer in this.cachedEspNLPs)
				{
					bool flag2 = networkedLocomotionPlayer == null;
					if (!flag2)
					{
						Vector3 vector;
						try
						{
							vector = networkedLocomotionPlayer.RigidbodyPosition;
						}
						catch
						{
							vector = networkedLocomotionPlayer.transform.position;
						}
						bool flag3 = vector == Vector3.zero;
						if (!flag3)
						{
							Vector3 vector2 = main.WorldToScreenPoint(vector);
							Vector3 vector3 = main.WorldToScreenPoint(vector + Vector3.up * 1.8f);
							bool flag4 = vector2.z <= 0f || vector3.z <= 0f;
							if (!flag4)
							{
								float num2 = (float)Screen.height - vector2.y;
								float num3 = (float)Screen.height - vector3.y;
								float num4 = Mathf.Abs(num2 - num3);
								float num5 = num4 * 0.5f;
								bool flag5 = num4 < 5f || num5 < 5f;
								if (!flag5)
								{
									float num6 = vector2.x - num5 / 2f;
									float num7 = num3;
									float num8 = Vector3.Distance(main.transform.position, vector);
									string arg = "???";
									Color color2 = color;
									try
									{
										PlayerState pstate = networkedLocomotionPlayer.PState;
										bool flag6 = pstate != null;
										if (flag6)
										{
											bool flag7 = !string.IsNullOrEmpty(pstate.DisplayName);
											if (flag7)
											{
												arg = pstate.DisplayName;
											}
											switch (pstate.UniversalGameRole)
											{
											case 1:
												color2 = new Color(0.4f, 0.7f, 1f);
												break;
											case 2:
												color2 = new Color(1f, 0.2f, 0.2f);
												break;
											case 3:
												color2 = new Color(1f, 0.75f, 0.1f);
												break;
											case 4:
												color2 = new Color(0.7f, 0f, 0.9f);
												break;
											case 5:
												color2 = new Color(1f, 0.5f, 0.1f);
												break;
											case 6:
												color2 = new Color(1f, 0.9f, 0.1f);
												break;
											case 7:
												color2 = new Color(0f, 1f, 0.8f);
												break;
											case 8:
												color2 = new Color(0.8f, 0.8f, 1f);
												break;
											case 9:
												color2 = new Color(1f, 0.3f, 0.7f);
												break;
											case 10:
												color2 = new Color(0.2f, 1f, 0.4f);
												break;
											default:
												color2 = color;
												break;
											}
										}
									}
									catch
									{
									}
									string text = string.Format("{0}  [{1:0}m]", arg, num8);
									Rect rect;
									rect..ctor(num6 - 40f, num7 - 20f, num5 + 80f, 20f);
									Rect rect2;
									rect2..ctor(num6, num7, num5, num4);
									this.DrawOutlinedText(rect, text, color2, Color.black, style);
									bool flag8 = CheatMod.esp2DBox || CheatMod.esp2DBoxSkeleton;
									if (flag8)
									{
										this.DrawBox2D(rect2, color, 2f);
									}
									bool flag9 = CheatMod.esp3DBox || CheatMod.esp3DBoxSkeleton;
									if (flag9)
									{
										this.DrawBox3D(vector, main, color, 0.35f, 1.8f);
									}
									bool flag10 = CheatMod.esp2DBoxSkeleton || CheatMod.esp3DBoxSkeleton;
									if (flag10)
									{
										this.DrawFakeSkeleton(vector, main, color);
									}
								}
							}
						}
					}
				}
				GUI.color = Color.white;
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00006BE4 File Offset: 0x00004DE4
		private void DrawBox2D(Rect rect, Color color, float thickness)
		{
			GUI.color = color;
			GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, thickness), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(rect.x, rect.y + rect.height, rect.width, thickness), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(rect.x, rect.y, thickness, rect.height), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(rect.x + rect.width, rect.y, thickness, rect.height), Texture2D.whiteTexture);
			GUI.color = Color.white;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00006CAC File Offset: 0x00004EAC
		private void DrawBox3D(Vector3 worldPos, Camera cam, Color color, float halfWidth, float playerHeight)
		{
			Vector3 right = cam.transform.right;
			Vector3 forward = cam.transform.forward;
			right.y = 0f;
			bool flag = right.sqrMagnitude > 0.001f;
			if (flag)
			{
				right.Normalize();
			}
			else
			{
				right = Vector3.right;
			}
			forward.y = 0f;
			bool flag2 = forward.sqrMagnitude > 0.001f;
			if (flag2)
			{
				forward.Normalize();
			}
			else
			{
				forward = Vector3.forward;
			}
			Vector3 vector = right * halfWidth;
			Vector3 vector2 = forward * halfWidth;
			Vector3 vector3 = Vector3.up * playerHeight;
			Vector3[] array = new Vector3[]
			{
				worldPos - vector - vector2,
				worldPos + vector - vector2,
				worldPos + vector + vector2,
				worldPos - vector + vector2,
				worldPos - vector - vector2 + vector3,
				worldPos + vector - vector2 + vector3,
				worldPos + vector + vector2 + vector3,
				worldPos - vector + vector2 + vector3
			};
			Vector2[] array2 = new Vector2[8];
			for (int i = 0; i < 8; i++)
			{
				Vector3 vector4 = cam.WorldToScreenPoint(array[i]);
				bool flag3 = vector4.z <= 0f;
				if (flag3)
				{
					return;
				}
				array2[i] = new Vector2(vector4.x, (float)Screen.height - vector4.y);
			}
			GUI.color = color;
			this.DrawLine2D(array2[0], array2[1]);
			this.DrawLine2D(array2[1], array2[2]);
			this.DrawLine2D(array2[2], array2[3]);
			this.DrawLine2D(array2[3], array2[0]);
			this.DrawLine2D(array2[4], array2[5]);
			this.DrawLine2D(array2[5], array2[6]);
			this.DrawLine2D(array2[6], array2[7]);
			this.DrawLine2D(array2[7], array2[4]);
			this.DrawLine2D(array2[0], array2[4]);
			this.DrawLine2D(array2[1], array2[5]);
			this.DrawLine2D(array2[2], array2[6]);
			this.DrawLine2D(array2[3], array2[7]);
			GUI.color = Color.white;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00006FB8 File Offset: 0x000051B8
		private void DrawLine2D(Vector2 a, Vector2 b)
		{
			float num = Vector2.Distance(a, b);
			bool flag = num < 1f;
			if (!flag)
			{
				float num2 = Mathf.Atan2(b.y - a.y, b.x - a.x) * 57.29578f;
				Matrix4x4 matrix = GUI.matrix;
				GUIUtility.RotateAroundPivot(num2, a);
				GUI.DrawTexture(new Rect(a.x, a.y - 1f, num, 2f), Texture2D.whiteTexture);
				GUI.matrix = matrix;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00007040 File Offset: 0x00005240
		private void DrawFakeSkeleton(Vector3 worldPos, Camera cam, Color c)
		{
			Vector3 right = cam.transform.right;
			right.y = 0f;
			bool flag = right.sqrMagnitude < 0.001f;
			if (flag)
			{
				right = Vector3.right;
			}
			else
			{
				right.Normalize();
			}
			Vector3 b = worldPos + Vector3.up * 1.75f;
			Vector3 vector = worldPos + Vector3.up * 1.55f;
			Vector3 vector2 = worldPos + Vector3.up * 1.2f;
			Vector3 vector3 = worldPos + Vector3.up * 0.7f;
			Vector3 a = vector2 + right * -0.35f;
			Vector3 a2 = vector2 + right * 0.35f;
			Vector3 vector4 = vector2 + right * -0.55f + Vector3.down * 0.3f;
			Vector3 vector5 = vector2 + right * 0.55f + Vector3.down * 0.3f;
			Vector3 b2 = vector2 + right * -0.55f + Vector3.down * 0.6f;
			Vector3 b3 = vector2 + right * 0.55f + Vector3.down * 0.6f;
			Vector3 a3 = vector3 + right * -0.2f;
			Vector3 a4 = vector3 + right * 0.2f;
			Vector3 vector6 = vector3 + right * -0.22f + Vector3.down * 0.4f;
			Vector3 vector7 = vector3 + right * 0.22f + Vector3.down * 0.4f;
			Vector3 b4 = vector3 + right * -0.22f + Vector3.down * 0.7f;
			Vector3 b5 = vector3 + right * 0.22f + Vector3.down * 0.7f;
			GUI.color = c;
			this.DrawBone(worldPos, vector3, cam);
			this.DrawBone(vector3, vector2, cam);
			this.DrawBone(vector2, vector, cam);
			this.DrawBone(vector, b, cam);
			this.DrawBone(a, vector4, cam);
			this.DrawBone(vector4, b2, cam);
			this.DrawBone(a2, vector5, cam);
			this.DrawBone(vector5, b3, cam);
			this.DrawBone(a3, vector6, cam);
			this.DrawBone(vector6, b4, cam);
			this.DrawBone(a4, vector7, cam);
			this.DrawBone(vector7, b5, cam);
			GUI.color = Color.white;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00007314 File Offset: 0x00005514
		private void DrawBone(Vector3 a, Vector3 b, Camera cam)
		{
			Vector3 vector = cam.WorldToScreenPoint(a);
			Vector3 vector2 = cam.WorldToScreenPoint(b);
			bool flag = vector.z <= 0f || vector2.z <= 0f;
			if (!flag)
			{
				this.DrawLine2D(new Vector2(vector.x, (float)Screen.height - vector.y), new Vector2(vector2.x, (float)Screen.height - vector2.y));
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00007390 File Offset: 0x00005590
		private void DrawOutlinedText(Rect rect, string text, Color color, Color outline, GUIStyle style)
		{
			style.normal.textColor = outline;
			GUI.Label(new Rect(rect.x - 1f, rect.y, rect.width, rect.height), text, style);
			GUI.Label(new Rect(rect.x + 1f, rect.y, rect.width, rect.height), text, style);
			GUI.Label(new Rect(rect.x, rect.y - 1f, rect.width, rect.height), text, style);
			GUI.Label(new Rect(rect.x, rect.y + 1f, rect.width, rect.height), text, style);
			style.normal.textColor = color;
			GUI.Label(rect, text, style);
		}

		// Token: 0x04000001 RID: 1
		private static bool menuOpen = false;

		// Token: 0x04000002 RID: 2
		private static int activeTab = 0;

		// Token: 0x04000003 RID: 3
		private static readonly string[] tabs = new string[]
		{
			"Main",
			"Advanced",
			"TELEPORT",
			"Info"
		};

		// Token: 0x04000004 RID: 4
		private static Vector2 scrollPos = Vector2.zero;

		// Token: 0x04000005 RID: 5
		private static bool noClip = false;

		// Token: 0x04000006 RID: 6
		private static bool fullbright = false;

		// Token: 0x04000007 RID: 7
		private static bool noKillCD = false;

		// Token: 0x04000008 RID: 8
		private static bool invisibleOn = false;

		// Token: 0x04000009 RID: 9
		private static bool crasherOn = false;

		// Token: 0x0400000A RID: 10
		private static float speedMult = 1f;

		// Token: 0x0400000B RID: 11
		private static bool esp2DBox = false;

		// Token: 0x0400000C RID: 12
		private static bool esp3DBox = false;

		// Token: 0x0400000D RID: 13
		private static bool esp2DBoxSkeleton = false;

		// Token: 0x0400000E RID: 14
		private static bool esp3DBoxSkeleton = false;

		// Token: 0x0400000F RID: 15
		private static string debugName = "";

		// Token: 0x04000010 RID: 16
		private static string debugId = "";

		// Token: 0x04000011 RID: 17
		private static string debugPing = "";

		// Token: 0x04000012 RID: 18
		private static Vector3 debugPos = Vector3.zero;

		// Token: 0x04000013 RID: 19
		private float debugRefreshTimer = 0f;

		// Token: 0x04000014 RID: 20
		private List<PlayerState> cachedTeleportPlayers = new List<PlayerState>();

		// Token: 0x04000015 RID: 21
		private List<string> cachedTeleportNames = new List<string>();

		// Token: 0x04000016 RID: 22
		private float teleportRefreshTimer = 0f;

		// Token: 0x04000017 RID: 23
		private XRRig tpManager = null;

		// Token: 0x04000018 RID: 24
		private float _tpFindTimer = 0f;

		// Token: 0x04000019 RID: 25
		private Rigidbody _pendingRb = null;

		// Token: 0x0400001A RID: 26
		private CharacterController _pendingCc = null;

		// Token: 0x0400001B RID: 27
		private float _restoreTimer = 0f;

		// Token: 0x0400001C RID: 28
		private List<PlayerState> cachedAllPlayers = new List<PlayerState>();

		// Token: 0x0400001D RID: 29
		private float allPlayerRefreshTimer = 0f;

		// Token: 0x0400001E RID: 30
		private PlayerState _cachedLocalPlayer = null;

		// Token: 0x0400001F RID: 31
		private NetworkedLocomotionPlayer _cachedLocalNLP = null;

		// Token: 0x04000020 RID: 32
		private float _localPlayerRefreshTimer = 0f;

		// Token: 0x04000021 RID: 33
		private NetworkedKillBehaviour _cachedKillBehaviour = null;

		// Token: 0x04000022 RID: 34
		private float _killBehaviourFindTimer = 0f;

		// Token: 0x04000023 RID: 35
		private Vector3 _invisibleSavedPosition = Vector3.zero;

		// Token: 0x04000024 RID: 36
		private bool _invisibleWasOn = false;

		// Token: 0x04000025 RID: 37
		private bool _invisiblePositionSaved = false;

		// Token: 0x04000026 RID: 38
		private List<NetworkedLocomotionPlayer> cachedEspNLPs = new List<NetworkedLocomotionPlayer>();

		// Token: 0x04000027 RID: 39
		private float cacheTimer = 0f;

		// Token: 0x04000028 RID: 40
		private List<PlayerState> _crasherTargets = new List<PlayerState>();

		// Token: 0x04000029 RID: 41
		private float _crasherListTimer = 0f;

		// Token: 0x0400002A RID: 42
		private bool debugDumped = false;

		// Token: 0x0400002B RID: 43
		private Dictionary<string, float> _btnClickTimers = new Dictionary<string, float>();

		// Token: 0x0400002C RID: 44
		private const float BTN_CLICK_DURATION = 0.12f;

		// Token: 0x0400002D RID: 45
		private static GUIStyle titleStyle;

		// Token: 0x0400002E RID: 46
		private static GUIStyle subtitleStyle;

		// Token: 0x0400002F RID: 47
		private static GUIStyle tabActiveStyle;

		// Token: 0x04000030 RID: 48
		private static GUIStyle tabStyle;

		// Token: 0x04000031 RID: 49
		private static GUIStyle btnToggleOn;

		// Token: 0x04000032 RID: 50
		private static GUIStyle btnToggleOff;

		// Token: 0x04000033 RID: 51
		private static GUIStyle btnAction;

		// Token: 0x04000034 RID: 52
		private static GUIStyle btnGold;

		// Token: 0x04000035 RID: 53
		private static GUIStyle btnDanger;

		// Token: 0x04000036 RID: 54
		private static GUIStyle labelStyle;

		// Token: 0x04000037 RID: 55
		private static GUIStyle sectionStyle;

		// Token: 0x04000038 RID: 56
		private static GUIStyle valueStyle;

		// Token: 0x04000039 RID: 57
		private static GUIStyle footerStyle;

		// Token: 0x0400003A RID: 58
		private static GUIStyle hintStyle;

		// Token: 0x0400003B RID: 59
		private static GUIStyle badgeStyle;

		// Token: 0x0400003C RID: 60
		private static bool stylesInit = false;

		// Token: 0x0400003D RID: 61
		private static readonly BindingFlags _priv = BindingFlags.Instance | BindingFlags.NonPublic;
	}
}
