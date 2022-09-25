import { Transaction } from "~~/models/transaction";
import { Result } from "./result";
import { parseISO } from "date-fns";

export function createApi() {
  const baseUrl = "https://localhost:5001/";

  async function fetchFromApi<T>(
    path: string,
    mapper: (dto: any) => T
  ): Promise<Result<T>> {
    try {
      const response = await fetch(baseUrl + path);
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

  async function postToApi(
    path: string,
    data: any
  ) {
    const response = await fetch(baseUrl + path, {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(data)
    });

    return response;
  }

  async function deleteFromApi(path: string) {
    const response = await fetch(baseUrl + path, {
      method: 'DELETE',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
    });

    return response;
  }

  return {
    getTransaction: (id: number) =>
      fetchFromApi<Transaction>(`transactions/${id}`, toTransaction),
    getTransactions: () =>
      fetchFromApi<Transaction[]>("transactions", (dtos: any) =>
        dtos.map(toTransaction)
      ),
    postTransaction: (transaction: any) => postToApi("transactions", transaction),
    deleteTransaction: (id: number) => deleteFromApi(`transactions/${id}`)
  };
}

function toTransaction(dto: any) {
  return {
    ...dto,
    date: parseISO(dto.date),
  };
}
