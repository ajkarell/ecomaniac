import { Transaction } from "~~/models/transaction";
import { Result } from "./result";
import { parseISO } from "date-fns";

export function createApi() {
  async function fetchFromApi<T>(
    path: string,
    mapper: (dto: any) => T
  ): Promise<Result<T>> {
    try {
      const response = await fetch(`https://localhost:5001/${path}`);
      if (response.ok) {
        const data = mapper(await response.json());
        return { ok: true, value: data };
      } else {
        return {
          ok: false,
          error: `HTTP error: ${response.statusText} (${response.status})`,
        };
      }
    } catch {
      return { ok: false, error: "Failed to connect" };
    }
  }

  return {
    getTransaction: (id: number) =>
      fetchFromApi<Transaction>(`transactions/${id}`, toTransaction),
    getTransactions: () =>
      fetchFromApi<Transaction[]>("transactions", (dtos: any) =>
        dtos.map(toTransaction)
      ),
  };
}

function toTransaction(dto: any) {
  return {
    ...dto,
    date: parseISO(dto.date),
  };
}
