<template>
  <div v-if="transactions !== null" class="flex flex-wrap items-start">
    <div class="mr-4 px-4 py-2 rounded-lg shadow border border-gray-300">
      <span class="text-lg">Record transaction</span>
      <hr />
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
            <input-currency
              v-model="transactionAmount"
              ref="amountInputRef"
              class="shadow appearance-none border rounded py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              id="amount"
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
          :class="{ 'opacity-50': !isTransactionValid(transactionToRecord) }"
          :disabled="!isTransactionValid(transactionToRecord)"
        >
          Record
        </button>
      </form>
    </div>
    <div class="mr-4 px-4 py-2 rounded-lg shadow border border-gray-300">
      <span class="text-lg">Transactions</span>
      <hr />

      <div v-if="transactions.length > 0">
        <div v-for="dateTransactions in transactionsByDate">
          <h1 class="text-md font-bold">
            {{ formatTransactionDate(dateTransactions[0].date) }}
          </h1>
          <button
            v-for="transaction in dateTransactions"
            @click="gotoTransactionDetails(transaction.id)"
            class="w-full px-2 py-1 flex justify-between rounded hover:bg-gray-200"
          >
            <span class="mr-4">{{
              transaction.description.length > 0
                ? transaction.description
                : "&#8212;"
            }}</span>
            <span class="font-bold" :class="moneyClass(transaction.amount)">
              {{ formatEuro(transaction.amount) }}
            </span>
          </button>
        </div>
      </div>
      <span v-else class="font-light italic">No transactions.</span>
    </div>
  </div>
  <h1 v-else class="text-2xl font-medium text-red-600">
    Ei yhteyttä palvelimeen.
  </h1>
</template>

<script setup lang="ts">
import { createApi } from "~~/api";
import { Transaction, isTransactionValid } from "~~/models/transaction";
import { formatEuro, formatTransactionDate } from "~~/formatting";
import { format, parseISO } from "date-fns";

const { getTransactions, postTransaction } = createApi();

const transactions = ref<Transaction[] | null>(null);
const today = new Date();
today.setUTCHours(0, 0, 0, 0);

const transactionsByDate = computed<any | null>(() => {
  if (transactions.value === null) return null;

  const dates = {};

  transactions.value.forEach((transaction) => {
    const dateString = format(transaction.date, "yyyy-MM-dd");
    if (dateString in dates) {
      dates[dateString].push(transaction);
    } else {
      dates[dateString] = new Array(transaction);
    }
  });

  return dates;
});

const transactionDateString = ref<string>(format(today, "yyyy-MM-dd"));
const transactionDate = computed(() => {
  return parseISO(transactionDateString.value);
});

const transactionAmount = ref(0.0);
const transactionDescription = ref("");

const transactionToRecord = computed(() => {
  return {
    date: transactionDate.value,
    amount: transactionAmount.value,
    description: transactionDescription.value,
  } as Transaction;
});

const amountInputRef = ref(null);

function moneyClass(amount: number) {
  return amount > 0 ? "text-green-600" : amount < 0 ? "text-red-600" : "";
}

function gotoTransactionDetails(id: number) {
  navigateTo(`/transactions/edit/${id}`);
}

async function recordTransaction() {
  const response = await postTransaction({
    date: transactionDate.value,
    amount: transactionAmount.value,
    description: transactionDescription.value,
  });

  if (response.ok) {
    transactionDescription.value = "";
    amountInputRef.value.select();
  } else {
    const body = await response.json();
    console.error(body);
    alert(body);
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
  amountInputRef.value.select();
});
</script>

<style lang="postcss">
hr {
  @apply my-2;
}
</style>
