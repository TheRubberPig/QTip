import type { PiiStats } from "../interfaces/PiiStats";
import type { SubmitResponse } from "../interfaces/SubmitResponse";

const API_URL = "http://localhost:8080";

export const ApiService = {
  getStats: async (): Promise<PiiStats> => {
    const res = await fetch(`${API_URL}/pii/email/count`);
    if (!res.ok) throw new Error('Failed to fetch stats');
    return res.json();
  },

  submitText: async (text: string): Promise<SubmitResponse> => {
    const res = await fetch(`${API_URL}/pii/submit`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ text }),
    });
    if (!res.ok) throw new Error('Failed to submit text');
    return res.json();
  }
};