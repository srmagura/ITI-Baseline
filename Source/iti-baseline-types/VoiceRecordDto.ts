import { NotificationId } from "./NotificationId";
import { VoiceRecordId } from './VoiceRecordId'
import { VoiceStatus } from './VoiceStatus'



export const VoiceRecordDtoTypeName = 'VoiceRecordDto'
export interface VoiceRecordDto  { 
    id: VoiceRecordId
    notificationId: NotificationId
    status: VoiceStatus
    sentUtc: Date | null| undefined
    toAddress: string
    body: string
    retryCount: number
    nextRetryUtc: Date | null | undefined
}
