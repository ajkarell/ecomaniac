<template>
  <div v-if="transactions !== null" class="flex">
    <div class="mr-4 px-4 py-2 rounded-lg shadow border border-gray-300">
      <span class="text-lg">Record transaction</span>
      <hr class="mb-2" />
      <form @submit.prevent="recordTransaction" class="mb-2">
        <div class="flex flex-col">
          <div class="pr-3 pb-2">
            <label class="block text-gray-700 font-bold mb-2" for="date">
              Date
            </label>
            <input
              v-model="transactionDateString"
              class="shadow appearance-none border rounded py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="date"
              type="date"
            />
          </div>
          <div class="pr-3 pb-2">
            <label class="block text-gray-700 font-bold mb-2" for="amount">
              Amount
            </label>
            <input
              v-model="transactionAmount"
              ref="amountInput"
              class="shadow appearance-none border rounded py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="amount"
              type="number"
              step="0.01"
              placeholder="0.00 €"
            />
          </div>
          <div class="pb-2">
            <label class="block text-gray-700 font-bold mb-2" for="description">
              Description
            </label>
            <input
              v-model="transactionDescription"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="description"
              type="text"
              placeholder="Description"
            />
          </div>
        </div>
        <button
          type="submit"
          class="mt-2 px-4 py-2 bg-blue-500 hover:bg-blue-700 text-white rounded-lg border border-blue-700"
        >
          Record
        </button>
      </form>
    </div>
    <div class="px-4 py-2 rounded-lg shadow border border-gray-300">
      <span class="text-lg">Transactions</span>
      <hr class="mb-2" />
      <table class="table-auto">
        <thead>
          <tr class="text-left">
            <th>Date</th>
            <th>Description</th>
            <th>Amount</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="transaction in transactions"
            :key="transaction.id"
            class="cursor-pointer hover:bg-gray-200"
            @click="gotoTransactionDetails(transaction.id)"
          >
            <td class="pr-4">{{ formatTransactionDate(transaction.date) }}</td>
            <td class="pr-4">{{ transaction.description }}</td>
            <td
              class="font-bold"
              :class="{
                'text-green-600': transaction.amount > 0,
                'text-red-600': transaction.amount < 0,
              }"
            >
              {{ formatEuro(transaction.amount) }}
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>

  <h1 v-else class="text-2xl font-medium text-red-600">
    Ei yhteyttä palvelimeen.
  </h1>
</template>

<script setup lang="ts">
import { createApi } from "~~/api";
import { Transaction } from "~~/models/transaction";
import { formatEuro, formatTransactionDate } from "~~/formatting";
import { format, parseISO } from "date-fns";

const { getTransactions, postTransaction } = createApi();

const transactions = ref<Transaction[] | null>(null);
const today = new Date();
today.setUTCHours(0, 0, 0, 0);

const transactionDateString = ref<string>(format(today, "yyyy-MM-dd"));
const transactionDate = computed(() => {
  return parseISO(transactionDateString.value);
});

const transactionAmount = ref(0.0);
const transactionDescription = ref("");

function gotoTransactionDetails(id: number) {
  navigateTo(`/transactions/edit/${id}`);
}

const amountInput = ref(null);

async function recordTransaction() {
  const response = await postTransaction({
    date: transactionDate.value,
    amount: transactionAmount.value,
    description: transactionDescription.value,
  });

  if (response.ok) {
    transactionDescription.value = "";
    amountInput.value.select();
  } else {
    alert(await response.json());
  }

  await fetchTransactions();
}

async function fetchTransactions() {
  const result = await getTransactions();
  if (result.ok) {
    transactions.value = result.value;
  }
}

onMounted(async () => {
  await fetchTransactions();
  amountInput.value.select();
});
</script>
