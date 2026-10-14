import math, random, struct, sys, wave
R = 22050
out = sys.argv[1]

def salvar(nome, amostras):
    with wave.open(f"{out}/{nome}.wav", "w") as w:
        w.setnchannels(1); w.setsampwidth(2); w.setframerate(R)
        w.writeframes(b"".join(struct.pack("<h", int(max(-1, min(1, s)) * 30000)) for s in amostras))

def tom(freq, dur, vol=0.5, forma="sen", queda=4.0):
    n = int(R * dur)
    res = []
    for i in range(n):
        t = i / R
        f = freq(t) if callable(freq) else freq
        fase = 2 * math.pi * f * t
        s = math.sin(fase) if forma == "sen" else (1 if math.sin(fase) > 0 else -1) * 0.5
        res.append(s * vol * math.exp(-queda * t) * min(1, i / 200))
    return res

random.seed(1)
salvar("colocar", tom(lambda t: 600 + 900 * t, 0.12, 0.6, queda=25))
salvar("quebrar", [random.uniform(-1, 1) * 0.7 * math.exp(-18 * i / R) for i in range(int(R * 0.35))])
salvar("queda", tom(lambda t: 500 - 600 * t, 0.7, 0.5, "quad", queda=2))
vit = []
for f in (523, 659, 784, 1046):
    vit += tom(f, 0.15, 0.5, "quad", queda=6)
vit += tom(1046, 0.5, 0.5, "quad", queda=4)
salvar("vitoria", vit)

# Música: 8 compassos em Dó, melodia quadrada + baixo senoidal, 120 bpm.
bpm, passo = 120, 0.25
melodia = [72,76,79,76, 74,77,81,77, 72,76,79,84, 79,76,74,72,
           69,72,76,72, 71,74,79,74, 72,76,79,76, 74,71,67,0]
baixo = [48,48,53,53,45,45,43,43]
freq = lambda m: 440 * 2 ** ((m - 69) / 12)
dur_nota = 60 / bpm * passo * 2
total = int(R * dur_nota * len(melodia))
mus = [0.0] * total
for k, m in enumerate(melodia):
    if m:
        for j, s in enumerate(tom(freq(m), dur_nota, 0.18, "quad", queda=3)):
            mus[int(k * dur_nota * R) + j] += s
for k, m in enumerate(baixo):
    d = dur_nota * 4
    for j, s in enumerate(tom(freq(m), d, 0.35, queda=1)):
        idx = int(k * d * R) + j
        if idx < total: mus[idx] += s
salvar("musica", mus)
