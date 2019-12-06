import { NotificationId } from "./NotificationId";
import { SmsRecordId } from './SmsRecordId'
import { SmsStatus } from './SmsStatus'

export const SmsRecordDtoTypeName = 'SmsRecordDto'
export interface SmsRecordDto  { 
    id: SmsRecordId
    notificationId: NotificationId
    status: SmsStatus
    sentUtc: Date | null| undefined
    toAddress: string
    body: string
    retryCount: number
    nextRetryUtc: Date | null| undefined
}
